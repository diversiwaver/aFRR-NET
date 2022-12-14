using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace TestWebAPI.Stubs;

internal class BidStub : IBidDataAccess
{
    public Task<int> CreateAsync(Bid entity)
    {
        return Task.FromResult(entity.Id);
    }

    public Task<bool> DeleteAsync(params dynamic[] id)
    {
        return Task.FromResult(true);
    }

    public Task<IEnumerable<Bid>> GetAllAsync()
    {
        IEnumerable<Bid> bids = new List<Bid>() {
            new Bid() {Id = 0, CurrencyId = 0, ExternalId = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0530"), FromUtc = new DateTime(2022,12,12,10,00,0), ToUtc = new DateTime(2022,12,12,11,00,0), Price = 15, QuantityMw = 10},
            new Bid() {Id = 1, CurrencyId = 1, ExternalId = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0531"), FromUtc = new DateTime(2022,12,12,11,00,0), ToUtc = new DateTime(2022,12,12,12,00,0), Price = 25, QuantityMw = 15},
            new Bid() {Id = 2, CurrencyId = 0, ExternalId = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0532"), FromUtc = new DateTime(2022,12,12,12,00,0), ToUtc = new DateTime(2022,12,12,13,00,0), Price = 20, QuantityMw = 25},
            new Bid() {Id = 3, CurrencyId = 1, ExternalId = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0533"), FromUtc = new DateTime(2022,12,12,13,00,0), ToUtc = new DateTime(2022,12,12,14,00,0), Price = 15, QuantityMw = 20}
        };
        return Task.FromResult(bids);
    }

    public Task<Bid> GetAsync(params dynamic[] id)
    {
        return Task.FromResult(new Bid() { Id = 0, CurrencyId = 0, ExternalId = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0530"), FromUtc = new DateTime(2022, 12, 12, 10, 00, 0), ToUtc = new DateTime(2022, 12, 12, 11, 00, 0), Price = 15, QuantityMw = 10 });
    }

    public Task<bool> UpdateAsync(Bid entity)
    {
        return Task.FromResult(true);
    }
}
