using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Models;
using ReserGo.Common.Requests.Products.Event;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Attributes;
using ReserGo.Shared;

namespace ReserGo.WebAPI.Controllers.Administration.Products;

[ApiController]
[Tags("Offers | Event")]
[AdminOnly]
[Route("api/administration/offers/events/")]
public class EventOfferController : ControllerBase {
    private readonly IBookingEventService _bookingEventService;
    private readonly ILogger<EventOfferController> _logger;
    private readonly IEventOfferService _occasionOfferService;
    private readonly ISecurity _security;

    public EventOfferController(ILogger<EventOfferController> logger,
        IEventOfferService occasionOfferService,
        ISecurity security, IBookingEventService bookingEventService) {
        _logger = logger;
        _occasionOfferService = occasionOfferService;
        _security = security;
        _bookingEventService = bookingEventService;
    }

    /// <summary>
    ///     Create a new @event offer.
    /// </summary>
    /// <param name="request">The @event offer creation request containing necessary information.</param>
    /// <returns>The created @event offer object.</returns>
    /// <response code="201">Event offer created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Create(EventOfferCreationRequest request) {
        try {
            var data = await _occasionOfferService.Create(request);

            var resource = new Resource<EventOfferDto> {
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
                    },
                    new() {
                        Href = Url.Action(nameof(Delete), new { id = data.Id }),
                        Rel = "delete",
                        Method = "DELETE"
                    }
                }
            };

            return Created("create", resource);
        }
        catch (InvalidDataException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while creating the @event offer.");
            return StatusCode(StatusCodes.Status500InternalServerError, Consts.UnexpectedError);
        }
    }


    /// <summary>
    ///     Retrieve a @event offer by its ID.
    /// </summary>
    /// <param name="id">The ID of the @event offer.</param>
    /// <returns>The @event offer object.</returns>
    /// <response code="200">Event offer found and returned.</response>
    /// <response code="404">Event offer not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<EventOfferDto>>> GetById(Guid id) {
        try {
            var occasionOffer = await _occasionOfferService.GetById(id);
            if (occasionOffer == null) return NotFound($"Event offer with ID {id} not found.");

            var resource = new Resource<EventOfferDto> {
                Data = occasionOffer,
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
                    },
                    new() {
                        Href = Url.Action(nameof(Delete), new { id }),
                        Rel = "delete",
                        Method = "DELETE"
                    }
                }
            };

            return Ok(resource);
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the @event offer.");
            return StatusCode(StatusCodes.Status500InternalServerError, Consts.UnexpectedError);
        }
    }

    /// <summary>
    ///     Retrieve @event offers for the connected user.
    /// </summary>
    /// <returns>A list of @event offers associated with the connected user.</returns>
    /// <response code="200">Event offers retrieved successfully.</response>
    /// <response code="401">User not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("my-offers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<IEnumerable<Resource<EventOfferDto>>>>> GetOffersForConnectedUser() {
        try {
            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) return Unauthorized("User not authenticated");

            var occasionOffers = await _occasionOfferService.GetEventsByUserId(connectedUser.UserId);

            var resources = occasionOffers.Select(offer => new Resource<EventOfferDto> {
                Data = offer,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetById), new { id = offer.Id }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new() {
                        Href = Url.Action(nameof(Update), new { id = offer.Id }),
                        Rel = "update",
                        Method = "PUT"
                    },
                    new() {
                        Href = Url.Action(nameof(Delete), new { id = offer.Id }),
                        Rel = "delete",
                        Method = "DELETE"
                    }
                }
            });

            var resourceCollection = new Resource<IEnumerable<Resource<EventOfferDto>>> {
                Data = resources,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetOffersForConnectedUser)),
                        Rel = "self",
                        Method = "GET"
                    }
                }
            };

            return Ok(resourceCollection);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving @event offers for the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, Consts.UnexpectedError);
        }
    }

    /// <summary>
    ///     Update an existing @event offer.
    /// </summary>
    /// <param name="id">The ID of the @event offer to update.</param>
    /// <param name="request">The @event offer update request.</param>
    /// <returns>The updated @event offer object.</returns>
    /// <response code="200">Event offer updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Event offer not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Resource<EventOfferDto>>> Update(Guid id, EventOfferUpdateRequest request) {
        try {
            var updatedOffer = await _occasionOfferService.Update(id, request);

            var resource = new Resource<EventOfferDto> {
                Data = updatedOffer,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetById), new { id }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new() {
                        Href = Url.Action(nameof(Delete), new { id }),
                        Rel = "delete",
                        Method = "DELETE"
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
    ///     Remove a @event offer by its ID.
    /// </summary>
    /// <param name="id">The ID of the @event offer to remove.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">Event offer removed successfully.</response>
    /// <response code="404">Event offer not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(Guid id) {
        try {
            await _occasionOfferService.Delete(id);
            return NoContent();
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while deleting the @event offer.");
            return StatusCode(StatusCodes.Status500InternalServerError, Consts.UnexpectedError);
        }
    }


    /// <summary>
    ///     Retrieve all bookings related to the admin's offers.
    /// </summary>
    /// <returns>
    ///     - **200 OK**: If the bookings are successfully retrieved.
    ///     - **401 Unauthorized**: If the admin is not authenticated.
    ///     - **500 Internal Server Error**: If an unexpected error occurs.
    /// </returns>
    /// <response code="200">Bookings retrieved successfully.</response>
    /// <response code="401">Admin is not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("my-bookings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBookingsForAdminOffers() {
        try {
            var admin = _security.GetCurrentUser();
            if (admin == null) return Unauthorized();

            var bookings = await _bookingEventService.GetBookingsByAdminId(admin.UserId);

            var bookingsWithLinks = bookings.Select(booking => new Resource<BookingEventDto> {
                Data = booking,
                Links = GenerateLinks(booking.Id)
            });

            var resourceCollection = new Resource<IEnumerable<Resource<BookingEventDto>>> {
                Data = bookingsWithLinks,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetBookingsForAdminOffers)),
                        Rel = "self",
                        Method = "GET"
                    }
                }
            };

            return Ok(resourceCollection);
        }
        catch (Exception e) {
            _logger.LogError(e, "An unexpected error occurred while retrieving bookings for admin offers");
            return StatusCode(StatusCodes.Status500InternalServerError, Consts.UnexpectedError);
        }
    }

    private List<Link> GenerateLinks(Guid bookingId) {
        return new List<Link> {
            new() {
                Href = Url.Action(nameof(GetBookingsForAdminOffers), new { id = bookingId }),
                Rel = "self",
                Method = "GET"
            },
            new() {
                Href = Url.Action("DeleteBooking", new { id = bookingId }),
                Rel = "delete",
                Method = "DELETE"
            }
        };
    }
}