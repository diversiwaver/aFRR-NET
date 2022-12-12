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
    IBidDataAccess _bidDataAccess;

    public BidsController(IBidDataAccess bidDataAccess)
    {
        _bidDataAccess = bidDataAccess;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BidDTO>> GetByIdAsync(int id)
    {
        Bid bid = await _bidDataAccess.GetAsync(id);
        if (bid == null)
        {
            return NotFound();
        }
        else
        {
            BidDTO bidDto = DTOConverter<Bid, BidDTO>.From(bid);
            return Ok(bidDto);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BidDTO>>> GetAllAsync()
    {
        IEnumerable<Bid> bids = await _bidDataAccess.GetAllAsync();
        IEnumerable<BidDTO> bidDtos = DTOConverter<Bid, BidDTO>.FromList(bids);
        return Ok(bidDtos);
    }
}
