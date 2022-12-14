using BaseDataAccess.Interfaces;
using BaseDataAccess.Models;
using System.Data.SqlClient;
using DataAccessLayer;

namespace TestDataAccess.Tests;

public class TestSignalDataAccess
{
    private ISignalDataAccess _dataAccess;
    private int _lastCreatedModelId;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _lastCreatedModelId = -1;
        _dataAccess = DataAccessFactory.GetDataAccess<ISignalDataAccess>(Configuration.CONNECTION_STRING_TEST);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _dataAccess.DeleteAsync(_lastCreatedModelId);
    }

    [Test]
    [Order(1)]
    public async Task SignalDataAccess_ShouldReturnAutogeneratedID_WhenCreatingASignal()
    {
        //Arrange
        Signal signal = new()
        {
            ReceivedUtc = DateTime.UtcNow,
            SentUtc = DateTime.UtcNow.AddHours(1),
            QuantityMw = 10,
            DirectionId = 0,
            BidId = 0
        };

        //Act
        _lastCreatedModelId = await _dataAccess.CreateAsync(signal);

        //Assert
        Assert.That(_lastCreatedModelId, Is.Not.EqualTo(-1), $"Failed to insert a new Signal.");
    }

    [Test]
    [Order(2)]
    public async Task SignalDataAccess_ShouldGetLastCreatedSignal_WhenGivenItsId()
    {
        //Arrange
        Signal newSignal;

        //Act
        newSignal = await _dataAccess.GetAsync(_lastCreatedModelId);

        //Assert
        Assert.That(newSignal, Is.Not.Null, $"No Signal was retrieved with ID: '{_lastCreatedModelId}'");
    }

    [Test]
    [Order(3)]
    public async Task SignalDataAccess_ShouldGetAllSignals()
    {
        //Arrange
        IEnumerable<Signal> signals;

        //Act
        signals = await _dataAccess.GetAllAsync();

        //Assert
        Assert.That(signals, Is.Not.Empty, $"Failed to retrieve all signals!");
    }

    [Test]
    [Order(4)]
    public async Task SignalDataAccess_ShouldReturnTrueAndTheNewSignal_WhenUpdatingAndGettingSignal()
    {
        //Arrange
        int newDirectionId = 1;
        bool isUpdated;
        Signal signal = await _dataAccess.GetAsync(_lastCreatedModelId);
        int oldDirectionId = signal.DirectionId;

        signal = new()
        {
            Id = _lastCreatedModelId,
            ReceivedUtc = signal.ReceivedUtc,
            SentUtc = signal.SentUtc,
            QuantityMw = signal.QuantityMw,
            DirectionId = newDirectionId,
            BidId = signal.BidId
        };

        //Act
        isUpdated = await _dataAccess.UpdateAsync(signal);
        Signal refoundSignal = await _dataAccess.GetAsync(signal.Id);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(isUpdated, Is.True, $"Failed to update Signal with ID: '{_lastCreatedModelId}'");
            Assert.That(refoundSignal.DirectionId, Is.Not.EqualTo(oldDirectionId), $"The direction id was not changed");
            Assert.That(refoundSignal.DirectionId, Is.EqualTo(signal.DirectionId), $"Getting the updated signal from the database returned a different one.");
        });
    }

    [Test]
    [Order(5)]
    public async Task SignalDataAccess_ShouldReturnTrue_WhenDeletingSignal()
    {
        //Arrange
        bool isDeleted;

        //Act
        isDeleted = await _dataAccess.DeleteAsync(_lastCreatedModelId);

        //Assert
        Assert.That(isDeleted, Is.True, $"Failed to delete Signal with ID: '{_lastCreatedModelId}'");
    }

    [Test]
    [Order(6)]
    public async Task SignalDataAccess_ShouldThrowException_WhenGivenEmptySignal()
    {
        //Arrange
        int createdId = -1;
        Signal signal = new() { };

        //Act

        //Assert
        Assert.That(async () => createdId = await _dataAccess.CreateAsync(signal), Throws.Exception.TypeOf<Exception>(), $"Created signal when it should have failed!: '{createdId}'");
        await _dataAccess.DeleteAsync(createdId);
    }

    [Test]
    [Order(7)]
    public async Task SignalDataAccess_ShouldThrowArgumentException_WhenGivenInvalidBidId()
    {
        //Arrange
        int createdId = -1;
        Signal refoundSignal;
        Signal signal = new()
        {
            ReceivedUtc = DateTime.UtcNow,
            SentUtc = DateTime.UtcNow.AddHours(1),
            QuantityMw = 10,
            DirectionId = 0,
            BidId = -999
        };

        //Act
        try
        {
            createdId = await _dataAccess.CreateAsync(signal);
            refoundSignal = await _dataAccess.GetAsync(createdId);
            Assert.That(refoundSignal, Is.Null, $"Created Signal wasn't rollbacked! createdId {createdId}");
        }
        //Assert
        catch (Exception ex)
        {
            Assert.That(ex.InnerException, Is.TypeOf<SqlException>(), $"Invalid Exception thrown with BidId -999!");
        }

        if (createdId != -1) {
            await _dataAccess.DeleteAsync(createdId);
        }
    }
}