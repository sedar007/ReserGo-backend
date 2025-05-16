using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Hotel.Rooms;
using ReserGo.WebAPI.Attributes;
using ReserGo.Shared.Interfaces;
using ReserGo.Common.Models;

namespace ReserGo.WebAPI.Controllers.Administration.Products;

[ApiController]
[Tags("Room | Rooms")]
[AdminOnly]
[Route("api/administration/hotels/rooms/")]
public class RoomController : ControllerBase {
    private readonly ILogger<RoomController> _logger;
    private readonly IRoomService _roomService;
    private readonly ISecurity _security;

    public RoomController(ILogger<RoomController> logger, IRoomService roomService, ISecurity security) {
        _logger = logger;
        _roomService = roomService;
        _security = security;
    }

    /// <summary>
    /// Create a new room.
    /// </summary>
    /// <param name="request">The room creation request containing necessary information.</param>
    /// <returns>The created room object.</returns>
    /// <response code="201">Room created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Create(RoomCreationRequest request) {
        try {
            var data = await _roomService.Create(request);

            var resource = new Resource<RoomDto> {
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
            _logger.LogError(ex, "An error occurred while creating the room.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Retrieve an room by their ID.
    /// </summary>
    /// <param name="id">The ID of the room.</param>
    /// <returns>The room object.</returns>
    /// <response code="200">Room found and returned.</response>
    /// <response code="404">Room not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<RoomDto>>> GetById(Guid id) {
        try {
            var room = await _roomService.GetById(id);
            if (room == null) return NotFound($"Room with ID {id} not found.");

            var resource = new Resource<RoomDto> {
                Data = room,
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
            _logger.LogError(ex, "An error occurred while retrieving the room.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
    

    /// <summary>
    /// Retrieve rooms for the connected user.
    /// </summary>
    /// <returns>A list of rooms associated with the connected user.</returns>
    /// <response code="200">Rooms retrieved successfully.</response>
    /// <response code="401">User not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("hotel/{hotelId}/rooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<IEnumerable<RoomDto>>>> GetRoomsByHotelId(Guid hotelId) {
        try {
            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) return Unauthorized("User not authenticated");

            var rooms = await _roomService.GetRoomsByHotelId(hotelId);

            var resource = new Resource<IEnumerable<RoomDto>> {
                Data = rooms,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetRoomsByHotelId), new { hotelId }),
                        Rel = "self",
                        Method = "GET"
                    }
                }
            };

            return Ok(resource);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving rooms for the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }


    /// <summary>
    /// Update an existing room.
    /// </summary>
    ///  <param name="id">The stayId to search the object.</param>
    /// <param name="request">The room update request.</param>
    /// <returns>The updated room object.</returns>
    /// <response code="200">Room updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Room not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Resource<RoomDto>>> Update(Guid id, RoomUpdateRequest request) {
        try {
            var updatedRoom = await _roomService.Update(id, request);

            var resource = new Resource<RoomDto> {
                Data = updatedRoom,
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
    /// Remove an room by their ID.
    /// </summary>
    /// <param name="id">The ID of the room to remove.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">Room removed successfully.</response>
    /// <response code="404">Room not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(Guid id) {
        try {
            await _roomService.Delete(id);
            return NoContent();
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the room.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}