using aFRRService.DTOs;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.DataAccess;

public class RemoteSignalDataAccess:IRemoteSignalDataAccess
{
    public async Task<bool> SendSignal(SignalDTO signal)
    {
        //TODO: hit RemoteSignalAPI endpoint
        throw new NotImplementedException();
    }
}