using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Models;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Attributes;
using ReserGo.WebAPI.Controllers.Administration.Products;

namespace ReserGo.WebAPI.Controllers.Administration.Offers;

[ApiController]
[Tags("Offers | Hotel")]
[AdminOnly]
[Route("api/administration/offers/hotels/")]
public class HotelOfferController : ControllerBase {
    private readonly IBookingHotelService _bookingHotelService;
    private readonly IHotelOfferService _hotelOfferService;
    private readonly ILogger<HotelController> _logger;
    private readonly ISecurity _security;

    public HotelOfferController(ILogger<HotelController> logger, IHotelOfferService hotelOfferService,
        ISecurity security, IBookingHotelService bookingHotelService) {
        _logger = logger;
        _hotelOfferService = hotelOfferService;
        _security = security;
        _bookingHotelService = bookingHotelService;
    }

    /// <summary>
    ///     Create a new hotel offer.
    /// </summary>
    /// <param name="request">The hotel offer creation request containing necessary information.</param>
    /// <returns>The created hotel offer object.</returns>
    /// <response code="201">Hotel offer created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Create(HotelOfferCreationRequest request) {
        try {
            var data = await _hotelOfferService.Create(request);
            var resource = new Resource<HotelOfferDto> {
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
            _logger.LogError(ex, "An error occurred while creating the hotel offer.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }


    /// <summary>
    ///     Retrieve a hotel offer by its ID.
    /// </summary>
    /// <param name="id">The ID of the hotel offer.</param>
    /// <returns>The hotel offer object.</returns>
    /// <response code="200">Hotel offer found and returned.</response>
    /// <response code="404">Hotel offer not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<HotelOfferDto?>> GetById(Guid id) {
        try {
            var hotelOffer = await _hotelOfferService.GetById(id);
            if (hotelOffer == null) return NotFound($"Hotel offer with ID {id} not found.");
            // Implement the HATEOAS links here 
            var resource = new Resource<HotelOfferDto> {
                Data = hotelOffer,
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
            _logger.LogError(ex, "An error occurred while retrieving the hotel offer.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    ///     Retrieve hotel offers for the connected user.
    /// </summary>
    /// <returns>A list of hotel offers associated with the connected user.</returns>
    /// <response code="200">Hotel offers retrieved successfully.</response>
    /// <response code="401">User not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("my-offers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<IEnumerable<Resource<HotelOfferDto>>>>> GetOffersForConnectedUser() {
        try {
            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) return Unauthorized("User not authenticated");

            var hotelOffers = await _hotelOfferService.GetHotelsByUserId(connectedUser.UserId);

            var resources = hotelOffers.Select(offer => new Resource<HotelOfferDto> {
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

            var resourceCollection = new Resource<IEnumerable<Resource<HotelOfferDto>>> {
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
            _logger.LogError(ex, "An error occurred while retrieving hotel offers for the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    ///     Update an existing hotel offer.
    /// </summary>
    /// <param name="id">The ID of the hotel offer to update.</param>
    /// <param name="request">The hotel offer update request.</param>
    /// <returns>The updated hotel offer object.</returns>
    /// <response code="200">Hotel offer updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Hotel offer not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Resource<HotelOfferDto>>> Update(Guid id, HotelOfferUpdateRequest request) {
        try {
            var updatedOffer = await _hotelOfferService.Update(id, request);

            var resource = new Resource<HotelOfferDto> {
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
    ///     Remove a hotel offer by its ID.
    /// </summary>
    /// <param name="id">The ID of the hotel offer to remove.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">Hotel offer removed successfully.</response>
    /// <response code="404">Hotel offer not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(Guid id) {
        try {
            await _hotelOfferService.Delete(id);
            return NoContent();
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while deleting the hotel offer.");
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

            var bookings = await _bookingHotelService.GetBookingsByAdminId(admin.UserId);

            var bookingsWithLinks = bookings.Select(booking => new Resource<BookingHotelDto> {
                Data = booking,
                Links = GenerateLinks(booking.Id)
            });

            var resourceCollection = new Resource<IEnumerable<Resource<BookingHotelDto>>> {
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