
using aFRRService.DTOs;

namespace DataAccessLayer.Interfaces;

internal interface IPrioritizationDataAccess
{
    Task<SignalDTO> GetAsync(SignalDTO signalDTO);
}
