
using aFRRService.DTOs;

namespace DataAccessLayer.Interfaces;

public interface IPrioritizationDataAccess
{
    Task<SignalDTO> GetAsync(SignalDTO signalDTO);
}
