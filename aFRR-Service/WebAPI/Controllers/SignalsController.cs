using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using WebAPI.DTOs;
using WebAPI.DTOs.DTOConverters;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SignalsController : ControllerBase
{
    private readonly ILogger<SignalsController> _logger;
    private readonly ISignalDataAccess _signalDataAccess;

    public SignalsController(ISignalDataAccess signalDataAccess, ILogger<SignalsController> logger)
    {
        _signalDataAccess = signalDataAccess;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<int>> PostAsync(SignalDTO signalDto)
    {
        _logger.LogInformation("PostAsync method called with signalDto: {signalDto}", signalDto);
        Signal signal = DTOConverter<SignalDTO, Signal>.From(signalDto);
        int createdId;
        try
        {
            createdId = await _signalDataAccess.CreateAsync(signal);
            _logger.LogInformation("Calling _signalDataAccess.CreateAsync with signal: {signal} returned id {createdId}", signal, createdId);
            return Ok(createdId);
        }
        catch (ArgumentException exception)
        {
            _logger.LogWarning("ArgumentException caught in PostAsync method: {exception}", exception);
            return BadRequest();
        }
        catch (Exception exception)
        {
            if (exception.InnerException != null && exception.InnerException is ArgumentException)
            {
                _logger.LogWarning("Inner ArgumentException caught in PostAsync method: {exception}", exception.InnerException);
                return BadRequest();
            }
            else
            {
                _logger.LogWarning("Exception caught in PostAsync method: {exception}", exception);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SignalDTO>>> GetAllAsync()
    {
        _logger.LogInformation("Getting all signals async.");
        IEnumerable<Signal> signals = await _signalDataAccess.GetAllAsync();
        _logger.LogInformation("Retrieved {Count} signals from the database.", signals.Count());
       
         _logger.LogInformation("Converting Signal to SignalDTO.");
        IEnumerable<SignalDTO> signalDtos = DTOConverter<Signal, SignalDTO>.FromList(signals);
        _logger.LogInformation("Converted {Count} signals to SignalDTO objects.", signalDtos.Count());
        _logger.LogInformation("Finished getting all signals.");
        return Ok(signalDtos);
    }
}
