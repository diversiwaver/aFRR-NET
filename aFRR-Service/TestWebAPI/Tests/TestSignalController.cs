using WebAPI.Controllers;
using TestWebAPI.Stubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using aFRRService.DTOs;

namespace TestWebAPI.Tests;

public class TestSignalController
{
    SignalsController _webApiController;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _webApiController = new SignalsController(new SignalStub(), new Mock<ILogger<SignalsController>>().Object);
    }

    [Test]
    public async Task SignalsController_ShouldReturnCreatedSignalId()
    {
        //Arrange
        SignalDTO newSignalDto = new() { Id = 0, CurrencyId = 0, FromUtc = new DateTime(2022, 12, 12, 10, 00, 0), ToUtc = new DateTime(2022, 12, 12, 11, 00, 0), Price = 15, QuantityMw = 10, DirectionId = 0, BidId = 0 };
        //Act
        var idActionResult = (await _webApiController.PostAsync(newSignalDto)).Result;
        if (idActionResult is ObjectResult objRes)
        {
            // Assert
            Assert.That(objRes.StatusCode, Is.EqualTo(200), "Status code returned was not 200");

            int returnedId = (int)objRes.Value;
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
