using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace TestWebAPI.Stubs;

internal class SignalStub : ISignalDataAccess
{
    public Task<int> CreateAsync(Signal entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(params dynamic[] id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Signal>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Signal> GetAsync(params dynamic[] id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Signal entity)
    {
        throw new NotImplementedException();
    }
}
