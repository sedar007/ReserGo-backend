using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Hotel;

namespace ReserGo.WebAPI.Controllers.Administration.Products;

[ApiController]
[Tags("Products | Hotel")] 
[Route("api/administration/products/hotels/")]
public class HotelController : ControllerBase {
    
    private readonly ILogger<HotelController> _logger;
    private readonly IHotelService _hotelService;

    public HotelController(ILogger<HotelController> logger, IHotelService hotelService) {
        _logger = logger;
        _hotelService = hotelService;
    }
    
    /// <summary>
    /// Create a new hotel.
    /// </summary>
    /// <param name="request">The hotel creation request containing necessary information.</param>
    /// <returns>The created hotel object.</returns>
    /// <response code="201">Hotel created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Create(HotelCreationRequest request) {
        try {
            HotelDto data = await _hotelService.Create(request);
            return Created("create", data);
        }
        catch (InvalidDataException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the hotel.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
        
    }
    
    /// <summary>
    /// Retrieve a hotel by their ID.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <returns>The hotel object.</returns>
    /// <response code="200">Hotel found and returned.</response>
    /// <response code="404">Hotel not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("getById/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<HotelDto?>> GetById(int id) {
        try {
            var hotel = await _hotelService.GetById(id);
            return Ok(hotel);
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the hotel.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}
