using aFRRService.DTOs;

namespace DataAccessLayer.Interfaces;
public interface IRemoteSignalDataAccess
{
    Task<bool> SendSignal(SignalDTO signal);
}