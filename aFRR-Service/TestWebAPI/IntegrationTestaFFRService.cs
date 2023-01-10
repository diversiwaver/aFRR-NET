using aFRRService.DTOs;
using BaseDataAccess.Interfaces;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TestWebAPI.Stubs;
using WebAPI.Controllers;

namespace TestWebAPI;
internal class IntegrationTestaFFRService
{
    SignalsController _webApiController;
    Random _random;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        ISignalDataAccess signalDataAccess = DataAccessFactory.GetDataAccess<ISignalDataAccess>(Configuration.CONNECTION_STRING_TEST);
        _webApiController = new SignalsController(signalDataAccess, new Mock<ILogger<SignalsController>>().Object);
        _random = new Random();
    }

    [Test]
    public async Task SignalsController_ShouldReturnCreatedSignalId()
    {
        //Arrange
        decimal quantity = (decimal)(_random.NextDouble() * 15 + 10);
        SignalDTO newSignalDto = new() { Id = 0, ReceivedUtc = DateTime.UtcNow, SentUtc = DateTime.UtcNow.AddHours(1), QuantityMw = quantity, Direction = Direction.Up, BidId = 0 };
        //Act
        var idActionResult = (await _webApiController.PostAsync(newSignalDto)).Result;
        if (idActionResult is ObjectResult objRes)
        {
            // Assert
            Assert.That(objRes.StatusCode, Is.EqualTo(200), "Status code returned was not 200");

            int returnedId = (int)objRes.Value;
            Assert.That(returnedId, Is.Not.EqualTo(-1), "Returned Id was -1, indicating a fail in the data access");
            newSignalDto.Id = returnedId;
            Assert.That(returnedId, Is.EqualTo(newSignalDto.Id), "Signal wasn't created");
        }
        else if (idActionResult is StatusCodeResult scr)
        {
            // Assert
            Assert.That(scr.StatusCode, Is.EqualTo(200));
        }
    }

    [Test]
    public async Task SignalsController_ShouldGetAllSignals()
    {
        //Arrange
        IEnumerable<SignalDTO>? signalDtos;

        //Act
        var actionResult = (await _webApiController.GetAllAsync()).Result;
        ObjectResult? objectResult = actionResult as ObjectResult;
        signalDtos = objectResult.Value as IEnumerable<SignalDTO>;
        Assert.Multiple(() =>
        {
            //Assert
            Assert.That(signalDtos, Is.Not.Null, "signalDtos list was null");
            Assert.That(objectResult.StatusCode, Is.EqualTo(200), "Status code was not OK(200)");
            Assert.That(signalDtos.Any(), Is.True, "List is currently empty");
        });
    }
}
