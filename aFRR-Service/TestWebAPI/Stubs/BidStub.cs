using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace TestWebAPI.Stubs;

internal class BidStub : IBidDataAccess
{
    public Task<int> CreateAsync(Bid entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(params dynamic[] id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Bid>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Bid> GetAsync(params dynamic[] id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Bid entity)
    {
        throw new NotImplementedException();
    }
}
