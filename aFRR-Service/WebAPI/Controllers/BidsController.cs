using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.DTOs.DTOConverters;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BidsController : ControllerBase
{
    private readonly ILogger<BidsController> _logger;
    private readonly IBidDataAccess _bidDataAccess;

    public BidsController(IBidDataAccess bidDataAccess, ILogger<BidsController> logger)
    {
        _bidDataAccess = bidDataAccess;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BidDTO>> GetByIdAsync(int id)
    {
        _logger.LogInformation("GetByIdAsync method called with Id: {id}.", id);
        Bid bid = await _bidDataAccess.GetAsync(id);
        if (bid == null)
        {
            _logger.LogInformation("Bid not found. Returning NotFound result.");
            return NotFound();
        }
        else
        {
            _logger.LogInformation("Finished getting bid '{bid}' from data access layer.", bid);
            BidDTO bidDto = DTOConverter<Bid, BidDTO>.From(bid);
            _logger.LogInformation("Converted Bid to BidDTO '{bidDto}'", bidDto);
            return Ok(bidDto);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BidDTO>>> GetAllAsync()
    {
        _logger.LogInformation("GetAllAsync method called.");
        IEnumerable<Bid> bids = await _bidDataAccess.GetAllAsync();
        _logger.LogInformation("Finished getting {Count} bids from data access layer.", bids.Count());
        IEnumerable<BidDTO> bidDtos = DTOConverter<Bid, BidDTO>.FromList(bids);
        _logger.LogInformation("Converted {Count} Bids to BidDTOs", bidDtos.Count());
        return Ok(bidDtos);
    }
}
