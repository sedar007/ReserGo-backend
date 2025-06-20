using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;

namespace ReserGo.WebAPI.Controllers.FranceGouv;

[ApiController]
[Route("api/[controller]")]
public class FranceGouvController : ControllerBase {
    private readonly IFranceGouvService _franceGouvService;
    private readonly ILogger<FranceGouvController> _logger;

    public FranceGouvController(ILogger<FranceGouvController> logger, IFranceGouvService franceGouvService) {
        _logger = logger;
        _franceGouvService = franceGouvService;
    }

    /// <summary>
    ///     Retrieve a list of addresses based on a search query.
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
        if (string.IsNullOrWhiteSpace(query)) {
            _logger.LogWarning("Search query is null or empty.");
            return Ok(Array.Empty<object>());
        }

        try {
            var addresses = await _franceGouvService.SearchAddresses(query);
            if (addresses == null || !addresses.Any()) {
                _logger.LogWarning("No addresses found for the query: {Query}", query);
                return Ok(Array.Empty<object>());
            }

            return Ok(addresses);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error occurred while searching for addresses.");
            return Ok(Array.Empty<object>());
        }
    }
}