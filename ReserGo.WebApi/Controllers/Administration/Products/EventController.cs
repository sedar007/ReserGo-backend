using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Event;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Attributes;
using ReserGo.Common.Models;

namespace ReserGo.WebAPI.Controllers.Administration.Products;

[AdminOnly]
[ApiController]
[Tags("Products | Event")]
[Route("api/administration/products/events/")]
public class EventController : ControllerBase {
    private readonly ILogger<EventController> _logger;
    private readonly IEventService _eventService;
    private readonly ISecurity _security;

    public EventController(ILogger<EventController> logger, IEventService eventService,
        ISecurity security) {
        _logger = logger;
        _security = security;
        _eventService = eventService;
    }

    /// <summary>
    /// Create a new @event.
    /// </summary>
    /// <param name="request">The @event creation request containing necessary information.</param>
    /// <returns>The created @event object.</returns>
    /// <response code="201">Event created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Create(EventCreationRequest request) {
        try {
            var data = await _eventService.Create(request);

            var resource = new Resource<EventDto> {
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
            _logger.LogError(ex, "An error occurred while creating the @event.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Retrieve an @event by their ID.
    /// </summary>
    /// <param name="id">The ID of the @event.</param>
    /// <returns>The Event object.</returns>
    /// <response code="200">Event found and returned.</response>
    /// <response code="404">Event not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<EventDto>>> GetById(Guid id) {
        try {
            var occasion = await _eventService.GetById(id);
            if (occasion == null) return NotFound($"Event with ID {id} not found.");

            var resource = new Resource<EventDto> {
                Data = occasion,
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
            _logger.LogError(ex, "An error occurred while retrieving the @event.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Retrieve an @event by their StayId.
    /// </summary>
    /// <param name="id">The StayId of the @event.</param>
    /// <returns>The Event object.</returns>
    /// <response code="200">Event found and returned.</response>
    /// <response code="404">Event not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("stayId/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<EventDto>>> GetByStayId(long id) {
        try {
            var occasion = await _eventService.GetByStayId(id);
            if (occasion == null) return NotFound($"Event with StayId {id} not found.");

            var resource = new Resource<EventDto> {
                Data = occasion,
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
            _logger.LogError(ex, "An error occurred while retrieving the @event.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Retrieve events for the connected user.
    /// </summary>
    /// <returns>A list of events associated with the connected user.</returns>
    /// <response code="200">Events retrieved successfully.</response>
    /// <response code="401">User not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("my-events")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<IEnumerable<Resource<EventDto>>>>> GetEventsForConnectedUser() {
        try {
            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) return Unauthorized("User not authenticated");

            var occasions = await _eventService.GetEventsByUserId(connectedUser.UserId);

            var resources = occasions.Select(occasion => new Resource<EventDto> {
                Data = occasion,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetById), new { id = occasion.Id }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new() {
                        Href = Url.Action(nameof(Update), new { id = occasion.Id }),
                        Rel = "update",
                        Method = "PUT"
                    }
                }
            });

            var resourceCollection = new Resource<IEnumerable<Resource<EventDto>>> {
                Data = resources,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetEventsForConnectedUser)),
                        Rel = "self",
                        Method = "GET"
                    }
                }
            };

            return Ok(resourceCollection);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving events for the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Update an existing @event.
    /// </summary>
    ///  <param name="id">The stayId to search the object.</param>
    /// <param name="request">The Event update request.</param>
    /// <returns>The updated Event object.</returns>
    /// <response code="200">Event updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Event not found.</response>
    [HttpPut("stayId/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Resource<EventDto>>> Update(long id, EventUpdateRequest request) {
        try {
            var updatedEvent = await _eventService.Update(id, request);

            var resource = new Resource<EventDto> {
                Data = updatedEvent,
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
    /// Remove an Event by their ID.
    /// </summary>
    /// <param name="id">The ID of the Event to remove.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">Event removed successfully.</response>
    /// <response code="404">Event not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(Guid id) {
        try {
            await _eventService.Delete(id);
            return NoContent();
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the Event.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}