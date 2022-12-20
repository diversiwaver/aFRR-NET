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
            new Signal() {Id = 0, CurrencyId = 0, FromUtc = new DateTime(2022,12,12,10,00,0), ToUtc = new DateTime(2022,12,12,11,00,0), Price = 15, QuantityMw = 10, DirectionId = 0, BidId = 0},
            new Signal() {Id = 1, CurrencyId = 1, FromUtc = new DateTime(2022,12,12,11,00,0), ToUtc = new DateTime(2022,12,12,12,00,0), Price = 25, QuantityMw = 15, DirectionId = 1, BidId = 0},
            new Signal() {Id = 2, CurrencyId = 0, FromUtc = new DateTime(2022,12,12,12,00,0), ToUtc = new DateTime(2022,12,12,13,00,0), Price = 20, QuantityMw = 25, DirectionId = 0, BidId = 0},
            new Signal() {Id = 3, CurrencyId = 1, FromUtc = new DateTime(2022,12,12,13,00,0), ToUtc = new DateTime(2022,12,12,14,00,0), Price = 15, QuantityMw = 20, DirectionId = 1, BidId = 0}
        };
        return Task.FromResult(signals);
    }

    public Task<Signal> GetAsync(params dynamic[] id)
    {
        return Task.FromResult(new Signal() { Id = 0, CurrencyId = 0, FromUtc = new DateTime(2022, 12, 12, 10, 00, 0), ToUtc = new DateTime(2022, 12, 12, 11, 00, 0), Price = 15, QuantityMw = 10, DirectionId = 0, BidId = 0 });
    }

    public Task<bool> UpdateAsync(Signal entity)
    {
        return Task.FromResult(true);
    }
}
