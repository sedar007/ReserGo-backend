using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.WebAPI.Attributes;
using ReserGo.Shared.Interfaces;
using ReserGo.Common.Models;

namespace ReserGo.WebAPI.Controllers.Administration.Products;

[ApiController]
[Tags("Products | Hotel")]
[AdminOnly]
[Route("api/administration/products/hotels/")]
public class HotelController : ControllerBase {
    private readonly ILogger<HotelController> _logger;
    private readonly IHotelService _hotelService;
    private readonly ISecurity _security;

    public HotelController(ILogger<HotelController> logger, IHotelService hotelService, ISecurity security) {
        _logger = logger;
        _hotelService = hotelService;
        _security = security;
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
            var data = await _hotelService.Create(request);

            var resource = new Resource<HotelDto> {
                Data = data,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetById), new { id = data.Id }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new() {
                        Href = Url.Action(nameof(Update), new { id = data.Id }),
                        Rel = "update",
                        Method = "PUT"
                    }
                }
            };

            return Created("create", resource);
        }
        catch (InvalidDataException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while creating the hotel.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Retrieve an hotel by their ID.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <returns>The hotel object.</returns>
    /// <response code="200">Hotel found and returned.</response>
    /// <response code="404">Hotel not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<HotelDto>>> GetById(Guid id) {
        try {
            var hotel = await _hotelService.GetById(id);
            if (hotel == null) return NotFound($"Hotel with ID {id} not found.");

            var resource = new Resource<HotelDto> {
                Data = hotel,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetById), new { id }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new() {
                        Href = Url.Action(nameof(Update), new { id }),
                        Rel = "update",
                        Method = "PUT"
                    }
                }
            };

            return Ok(resource);
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the hotel.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Retrieve an hotel by their StayId.
    /// </summary>
    /// <param name="id">The StayId of the hotel.</param>
    /// <returns>The hotel object.</returns>
    /// <response code="200">Hotel found and returned.</response>
    /// <response code="404">Hotel not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("stayId/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<HotelDto>>> GetByStayId(long id) {
        try {
            var hotel = await _hotelService.GetByStayId(id);
            if (hotel == null) return NotFound($"Hotel with StayId {id} not found.");

            var resource = new Resource<HotelDto> {
                Data = hotel,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetByStayId), new { id }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new() {
                        Href = Url.Action(nameof(Update), new { id }),
                        Rel = "update",
                        Method = "PUT"
                    }
                }
            };

            return Ok(resource);
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the hotel.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Retrieve hotels for the connected user.
    /// </summary>
    /// <returns>A list of hotels associated with the connected user.</returns>
    /// <response code="200">Hotels retrieved successfully.</response>
    /// <response code="401">User not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("my-hotels")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<IEnumerable<Resource<HotelDto>>>>> GetHotelsForConnectedUser() {
        try {
            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) return Unauthorized("User not authenticated");

            var hotels = await _hotelService.GetHotelsByUserId(connectedUser.UserId);

            var resources = hotels.Select(hotel => new Resource<HotelDto> {
                Data = hotel,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetById), new { id = hotel.Id }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new() {
                        Href = Url.Action(nameof(Update), new { id = hotel.Id }),
                        Rel = "update",
                        Method = "PUT"
                    }
                }
            });

            var resourceCollection = new Resource<IEnumerable<Resource<HotelDto>>> {
                Data = resources,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetHotelsForConnectedUser)),
                        Rel = "self",
                        Method = "GET"
                    }
                }
            };

            return Ok(resourceCollection);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving hotels for the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }


    /// <summary>
    /// Update an existing hotel.
    /// </summary>
    ///  <param name="id">The stayId to search the object.</param>
    /// <param name="request">The hotel update request.</param>
    /// <returns>The updated hotel object.</returns>
    /// <response code="200">Hotel updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Hotel not found.</response>
    [HttpPut("stayId/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Resource<HotelDto>>> Update(long id, HotelUpdateRequest request) {
        try {
            var updatedHotel = await _hotelService.Update(id, request);

            var resource = new Resource<HotelDto> {
                Data = updatedHotel,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetById), new { id }),
                        Rel = "self",
                        Method = "GET"
                    }
                }
            };

            return Ok(resource);
        }
        catch (InvalidDataException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Remove an hotel by their ID.
    /// </summary>
    /// <param name="id">The ID of the hotel to remove.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">Hotel removed successfully.</response>
    /// <response code="404">Hotel not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(Guid id) {
        try {
            await _hotelService.Delete(id);
            return NoContent();
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