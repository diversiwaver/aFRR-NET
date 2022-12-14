using WebAPI.Controllers;
using TestWebAPI.Stubs;
using WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework.Internal;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestWebAPI.Tests;

public class TestBidController
{
    BidsController _webApiController;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _webApiController = new BidsController(new BidStub(), new Mock<ILogger<BidsController>>().Object);
    }

    [Test]
    public async Task BidsController_ShouldReturnBidDtoIdAndStatusCode200_WhenGettingValidBidId()
    {
        //Arrange
        //Act
        var actionResult = (await _webApiController.GetByIdAsync(0)).Result;
        Assert.That(actionResult, Is.Not.Null, "Action result was null");
        if (actionResult is ObjectResult objRes)
        {
            //Assert
            BidDTO? bid = objRes.Value as BidDTO;
            Assert.Multiple(() =>
            {
                Assert.That(objRes.StatusCode, Is.EqualTo(200), "Status code returned was not 200");
                Assert.That(bid, Is.Not.Null, "Returned Bid was null");
                Assert.That(bid.Id, Is.EqualTo(0), "Bid id didn't match");
            });
        }
        else if (actionResult is StatusCodeResult scr)
        {
            //Assert
            Assert.That(scr.StatusCode, Is.EqualTo(200));
        }
    }

    [Test]
    public async Task BidsController_ShouldGetAllBids()
    {
        //Arrange
        IEnumerable<BidDTO>? bidDtos;

        //Act
        var actionResult = (await _webApiController.GetAllAsync()).Result;
        ObjectResult? objectResult = actionResult as ObjectResult;
        bidDtos = objectResult.Value as IEnumerable<BidDTO>;
        Assert.Multiple(() =>
        {
            //Assert
            Assert.That(bidDtos, Is.Not.Null, "bidDtos list was null");
            Assert.That(objectResult.StatusCode, Is.EqualTo(200), "Status code was not OK(200)");
            Assert.That(bidDtos.Any(), Is.True, "List is currently empty");
        });
    }
}
