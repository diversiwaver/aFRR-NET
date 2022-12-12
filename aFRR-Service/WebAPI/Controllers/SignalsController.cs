using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.DTOs.DTOConverters;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SignalsController : ControllerBase
{
    ISignalDataAccess _signalDataAccess;

    public SignalsController(ISignalDataAccess signalDataAccess)
    {
        _signalDataAccess = signalDataAccess;
    }

    [HttpPost]
    public async Task<ActionResult<int>> PostAsync(SignalDTO signalDto)
    {
        Signal signal = DTOConverter<SignalDTO, Signal>.From(signalDto);
        int createdId;
        try
        {
            createdId = await _signalDataAccess.CreateAsync(signal);
            return Ok(createdId);
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
        catch (Exception exception)
        {
            if (exception.InnerException is ArgumentException)
            {
                return BadRequest();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SignalDTO>>> GetAllAsync()
    {
        IEnumerable<Signal> signals = await _signalDataAccess.GetAllAsync();
        IEnumerable<SignalDTO> signalDtos = DTOConverter<Signal, SignalDTO>.FromList(signals);
        return Ok(signalDtos);
    }
}
