using Microsoft.AspNetCore.Mvc;
using Carpet.Models.DTOs;
using Carpet.Repositories;
using Carpet.Mappers;

namespace Carpet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarpetController : ControllerBase
{
    private readonly ICarpetRepository _carpetRepository;
    private readonly ILogger<CarpetController> _logger;

    public CarpetController(
        ICarpetRepository carpetRepository,
        ILogger<CarpetController> logger)
    {
        _carpetRepository = carpetRepository;
        _logger = logger;
    }

    // GET: api/Carpet/search?term=...
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<CarpetDto>>> Search([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            var allCarpets = await _carpetRepository.GetAllAsync();
            return Ok(allCarpets.Select(CarpetMapper.ToDto));
        }

        var carpets = await _carpetRepository.SearchByNameAsync(term);
        return Ok(carpets.Select(CarpetMapper.ToDto));
    }

    // GET: api/Carpet
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarpetDto>>> GetAll()
    {
        var carpets = await _carpetRepository.GetAllAsync();
        return Ok(carpets.Select(CarpetMapper.ToDto));
    }
}

