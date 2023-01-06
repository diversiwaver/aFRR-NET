using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using aFRRService.DTOs;
using DataAccessLayer;
using DataAccessLayer.DataAccess;
using DataAccessLayer.Interfaces;
using Moq;
using Moq.Protected;

namespace TestDataAccess.Tests;

internal class TestPrioritizationDataAccess
{
    private IPrioritizationDataAccess _dataAccess;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        string signalDTOStringContent = """
            {
                "id": 1,
                "fromUtc": "2023-01-04T12:16:56.28Z",
                "toUtc": "2023-01-04T12:16:56.28Z",
                "price": 25,
                "currencyId": 0,
                "quantityMw": 20,
                "directionId": 1,
                "bidId": 0,
                "assetsToRegulate": [
                    {
                        "id": 33,
                        "assetGroupId": 2,
                        "location": null,
                        "capacityMw": 10,
                        "regulationPercentage": 100
                    },
                    {
                        "id": 21,
                        "assetGroupId": 0,
                        "location": null,
                        "capacityMw": 8,
                        "regulationPercentage": 100
                    },
                    {
                        "id": 31,
                        "assetGroupId": 2,
                        "location": null,
                        "capacityMw": 8,
                        "regulationPercentage": 25.00
                    }
                ]
            }
            """;
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            // Setup the PROTECTED method to mock
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            // prepare the expected response of the mocked http call
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(signalDTOStringContent),
            })
            .Verifiable();

        HttpClient mockHTTPClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://test.com/"),
        };

        _dataAccess = DataAccessFactory.GetDataAccess<IPrioritizationDataAccess>(mockHTTPClient);
    }

    [Test]
    public async Task PrioritizationDataAccess_ShouldReturnSignalDTOWithPrioritizedAssets_WhenQueried()
    {
        //Arrange
        SignalDTO signalDTO = new SignalDTO
        {
            Id = 1,
            FromUtc = DateTime.Parse("2023-01-04T12:16:56.28Z"),
            ToUtc = DateTime.Parse("2023-01-04T12:16:56.28Z"),
            Price = 25,
            CurrencyId = 0,
            QuantityMw = 20,
            DirectionId = 1,
            BidId = 0,
        };

        var expectedAssetsToRegulate = new List<AssetDTO>
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
            },
        };

        //Act
        var returnedSignal = (await _dataAccess.GetAsync(signalDTO));

        //Assert
        Assert.That(returnedSignal.AssetsToRegulate, Is.Not.Empty, "Returned SignalDTO has no Assets");
        Assert.That(returnedSignal.AssetsToRegulate, Is.EquivalentTo(expectedAssetsToRegulate).Using(new AssetDTOComparer()), "Returned assets do not match the assets expected");
    }
}

internal class AssetDTOComparer : IEqualityComparer<AssetDTO>
{
    public bool Equals(aFRRService.DTOs.AssetDTO x, aFRRService.DTOs.AssetDTO y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.Id == y.Id
            && x.AssetGroupId == y.AssetGroupId
            && x.Location == y.Location
            && x.CapacityMw == y.CapacityMw
            && x.RegulationPercentage == y.RegulationPercentage;
    }

    public int GetHashCode(aFRRService.DTOs.AssetDTO obj)
    {
        // Return a consistent value for objects with the same values
        return obj.Id.GetHashCode()
            ^ obj.AssetGroupId.GetHashCode()
            ^ obj.Location.GetHashCode()
            ^ obj.CapacityMw.GetHashCode()
            ^ obj.RegulationPercentage.GetHashCode();
    }
}

