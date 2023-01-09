using BaseDataAccess.Interfaces;
using BaseDataAccess.Models;

namespace TestWebAPI.Stubs;

internal class SignalStub : ISignalDataAccess
{
    public Task<int> CreateAsync(Signal entity)
    {
        return Task.FromResult(entity.Id);
    }

    public Task<bool> DeleteAsync(params dynamic[] id)
    {
        return Task.FromResult(true);
    }

    public Task<IEnumerable<Signal>> GetAllAsync()
    {
        IEnumerable<Signal> signals = new List<Signal>() {
            new Signal() {Id = 0, ReceivedUtc = new DateTime(2022,12,12,10,00,0), SentUtc = new DateTime(2022,12,12,11,00,0), QuantityMw = 10, DirectionId = 0, BidId = 0},
            new Signal() {Id = 1, ReceivedUtc = new DateTime(2022,12,12,11,00,0), SentUtc = new DateTime(2022,12,12,12,00,0), QuantityMw = 15, DirectionId = 1, BidId = 0},
            new Signal() {Id = 2, ReceivedUtc = new DateTime(2022,12,12,12,00,0), SentUtc = new DateTime(2022,12,12,13,00,0), QuantityMw = 25, DirectionId = 0, BidId = 0},
            new Signal() {Id = 3, ReceivedUtc = new DateTime(2022,12,12,13,00,0), SentUtc = new DateTime(2022,12,12,14,00,0), QuantityMw = 20, DirectionId = 1, BidId = 0}
        };
        return Task.FromResult(signals);
    }

    public Task<Signal> GetAsync(params dynamic[] id)
    {
        return Task.FromResult(new Signal() { Id = 0, ReceivedUtc = new DateTime(2022, 12, 12, 10, 00, 0), SentUtc = new DateTime(2022, 12, 12, 11, 00, 0), QuantityMw = 10, DirectionId = 0, BidId = 0 });
    }

    public Task<bool> UpdateAsync(Signal entity)
    {
        return Task.FromResult(true);
    }
}
