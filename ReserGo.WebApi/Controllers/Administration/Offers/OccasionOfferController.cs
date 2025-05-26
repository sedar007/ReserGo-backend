using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Occasion;
using ReserGo.WebAPI.Attributes;
using ReserGo.Shared.Interfaces;
using ReserGo.Common.Models;

namespace ReserGo.WebAPI.Controllers.Administration.Products;

[ApiController]
[Tags("Offers | Occasion")]
[AdminOnly]
[Route("api/administration/offers/occasions/")]
public class OccasionOfferController : ControllerBase {
    private readonly ILogger<OccasionController> _logger;
    private readonly IOccasionOfferService _occasionOfferService;
    private readonly ISecurity _security;
    private readonly IBookingOccasionService _bookingOccasionService;

    public OccasionOfferController(ILogger<OccasionController> logger, 
        IOccasionOfferService occasionOfferService,
        ISecurity security, IBookingOccasionService bookingOccasionService) {
        _logger = logger;
        _occasionOfferService = occasionOfferService;
        _security = security;
        _bookingOccasionService = bookingOccasionService;
    }

    /// <summary>
    /// Create a new occasion offer.
    /// </summary>
    /// <param name="request">The occasion offer creation request containing necessary information.</param>
    /// <returns>The created occasion offer object.</returns>
    /// <response code="201">Occasion offer created successfully.</response>
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
    public async Task<ActionResult> Create(OccasionOfferCreationRequest request) {
        try {
            var data = await _occasionOfferService.Create(request);

            var resource = new Resource<OccasionOfferDto> {
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
            _logger.LogError(ex, "An error occurred while creating the occasion offer.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }


    /// <summary>
    /// Retrieve a occasion offer by its ID.
    /// </summary>
    /// <param name="id">The ID of the occasion offer.</param>
    /// <returns>The occasion offer object.</returns>
    /// <response code="200">Occasion offer found and returned.</response>
    /// <response code="404">Occasion offer not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<OccasionOfferDto>>> GetById(Guid id) {
        try {
            var occasionOffer = await _occasionOfferService.GetById(id);
            if (occasionOffer == null) return NotFound($"Occasion offer with ID {id} not found.");

            var resource = new Resource<OccasionOfferDto> {
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
            _logger.LogError(ex, "An error occurred while retrieving the occasion offer.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Retrieve occasion offers for the connected user.
    /// </summary>
    /// <returns>A list of occasion offers associated with the connected user.</returns>
    /// <response code="200">Occasion offers retrieved successfully.</response>
    /// <response code="401">User not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("my-offers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<IEnumerable<Resource<OccasionOfferDto>>>>> GetOffersForConnectedUser() {
        try {
            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) return Unauthorized("User not authenticated");

            var occasionOffers = await _occasionOfferService.GetOccasionsByUserId(connectedUser.UserId);

            var resources = occasionOffers.Select(offer => new Resource<OccasionOfferDto> {
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

            var resourceCollection = new Resource<IEnumerable<Resource<OccasionOfferDto>>> {
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
            _logger.LogError(ex, "An error occurred while retrieving occasion offers for the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Update an existing occasion offer.
    /// </summary>
    /// <param name="id">The ID of the occasion offer to update.</param>
    /// <param name="request">The occasion offer update request.</param>
    /// <returns>The updated occasion offer object.</returns>
    /// <response code="200">Occasion offer updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Occasion offer not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Resource<OccasionOfferDto>>> Update(Guid id, OccasionOfferUpdateRequest request) {
        try {
            var updatedOffer = await _occasionOfferService.Update(id, request);

            var resource = new Resource<OccasionOfferDto> {
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
    /// Remove a occasion offer by its ID.
    /// </summary>
    /// <param name="id">The ID of the occasion offer to remove.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">Occasion offer removed successfully.</response>
    /// <response code="404">Occasion offer not found.</response>
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
            _logger.LogError(ex, "An error occurred while deleting the occasion offer.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
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

            var bookings = await _bookingOccasionService.GetBookingsByAdminId(admin.UserId);

            var bookingsWithLinks = bookings.Select(booking => new Resource<BookingOccasionDto> {
                Data = booking,
                Links = GenerateLinks(booking.Id)
            });

            var resourceCollection = new Resource<IEnumerable<Resource<BookingOccasionDto>>> {
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
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
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