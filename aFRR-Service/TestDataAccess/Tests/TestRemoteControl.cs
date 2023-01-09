using aFRRService.DTOs;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using Moq;
using Moq.Protected;
using System.Net;
using System.Reflection;

namespace TestDataAccess.Tests;
public class TestRemoteControl
{
    private IRemoteControlDataAccess _dataAccess;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
            })
            .Verifiable();

        HttpClient mockHTTPClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://test.com/"),
        };

        _dataAccess = DataAccessFactory.GetDataAccess<IRemoteControlDataAccess>(mockHTTPClient);
    }

    [Test]
    public async Task RemoteControlDataAccess_ShouldReturnTrue_WhenSendingSignal()
    {
        //Arrange
        SignalDTO signalDTO = new SignalDTO()
        {
            Id = 1,
            ReceivedUtc = DateTime.UtcNow,
            SentUtc = DateTime.UtcNow.AddHours(1),
            QuantityMw = 10,
            Direction = Direction.Up,
            BidId = 0,
            AssetsToRegulate = new List<AssetDTO>()
            {
                new AssetDTO
                {
                    Id = 33,
                    AssetGroupId = 2,
                    Location = null,
                    CapacityMw = 10,
                    RegulationPercentage = 100
                },
                new AssetDTO
                {
                    Id = 21,
                    AssetGroupId = 0,
                    Location = null,
                    CapacityMw = 8,
                    RegulationPercentage = 100
                },
                new AssetDTO
                {
                    Id = 31,
                    AssetGroupId = 2,
                    Location = null,
                    CapacityMw = 8,
                    RegulationPercentage = 25
                }
            }
        };

        //Act
        var returnedSignal = (await _dataAccess.SendAsync(signalDTO));

        //Assert
        Assert.That(returnedSignal, Is.True, "Sending signal returned False.");
    }   
}
