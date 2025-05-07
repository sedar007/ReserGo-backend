using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Models;
using ReserGo.Common.Requests.User;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Attributes;

namespace ReserGo.WebAPI.Controllers.FranceGouv;

[ApiController]
[Route("api/[controller]")]
public class FranceGouvController : ControllerBase {
    private readonly ILogger<FranceGouvController> _logger;
    private readonly IFranceGouvService _franceGouvService;

    public FranceGouvController(ILogger<FranceGouvController> logger, IFranceGouvService franceGouvService) {
        _logger = logger;
        _franceGouvService = franceGouvService;
    }

    /// <summary>
    /// Retrieve a list of addresses based on a search query.
    /// </summary>
    /// <param name="query">The search query to find addresses.</param>
    /// <returns>A list of matching addresses.</returns>
    /// <response code="200">Addresses retrieved successfully.</response>
    /// <response code="400">Invalid or missing query parameter.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("addresses/search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchAddresses([FromQuery] string query) {
        if (string.IsNullOrWhiteSpace(query)) return BadRequest("Query parameter is required.");

        try {
            var addresses = await _franceGouvService.SearchAddresses(query);
            if (addresses == null || !addresses.Any()) {
                return NotFound("No addresses found.");
            }

            return Ok(addresses);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error occurred while searching for addresses.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}