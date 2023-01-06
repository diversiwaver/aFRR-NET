using aFRRService.DTOs;

namespace DataAccessLayer.Interfaces;

public interface IRemoteControlDataAccess
{
    Task<bool> SendAsync(SignalDTO signalDto);
}
