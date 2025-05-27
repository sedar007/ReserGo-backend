using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.WebAPI.Attributes;
using ReserGo.Shared.Interfaces;
using ReserGo.Common.Models;

namespace ReserGo.WebAPI.Controllers.Administration.Products;

[ApiController]
[Tags("Offers | Restaurant")]
[AdminOnly]
[Route("api/administration/offers/restaurants/")]
public class RestaurantOfferController : ControllerBase {
    private readonly ILogger<RestaurantController> _logger;
    private readonly IRestaurantOfferService _restaurantOfferService;
    private readonly ISecurity _security;
    private readonly IBookingRestaurantService _bookingRestaurantService;

    public RestaurantOfferController(ILogger<RestaurantController> logger,
        IRestaurantOfferService restaurantOfferService, ISecurity security,
        IBookingRestaurantService bookingRestaurantService) {
        _logger = logger;
        _restaurantOfferService = restaurantOfferService;
        _security = security;
        _bookingRestaurantService = bookingRestaurantService;
    }

    /// <summary>
    /// Create a new restaurant offer.
    /// </summary>
    /// <param name="request">The restaurant offer creation request containing necessary information.</param>
    /// <returns>The created restaurant offer object.</returns>
    /// <response code="201">Restaurant offer created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Create(RestaurantOfferCreationRequest request) {
        try {
            var data = await _restaurantOfferService.Create(request);

            var resource = new Resource<RestaurantOfferDto> {
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
            _logger.LogError(ex, "An error occurred while creating the restaurant offer.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }


    /// <summary>
    /// Retrieve a restaurant offer by its ID.
    /// </summary>
    /// <param name="id">The ID of the restaurant offer.</param>
    /// <returns>The restaurant offer object.</returns>
    /// <response code="200">Restaurant offer found and returned.</response>
    /// <response code="404">Restaurant offer not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<RestaurantOfferDto>>> GetById(Guid id) {
        try {
            var restaurantOffer = await _restaurantOfferService.GetById(id);
            if (restaurantOffer == null) return NotFound($"Restaurant offer with ID {id} not found.");

            var resource = new Resource<RestaurantOfferDto> {
                Data = restaurantOffer,
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
            _logger.LogError(ex, "An error occurred while retrieving the restaurant offer.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Retrieve restaurant offers for the connected user.
    /// </summary>
    /// <returns>A list of restaurant offers associated with the connected user.</returns>
    /// <response code="200">Restaurant offers retrieved successfully.</response>
    /// <response code="401">User not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("my-offers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<IEnumerable<Resource<RestaurantOfferDto>>>>> GetOffersForConnectedUser() {
        try {
            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) return Unauthorized("User not authenticated");

            var restaurantOffers = await _restaurantOfferService.GetRestaurantsByUserId(connectedUser.UserId);

            var resources = restaurantOffers.Select(offer => new Resource<RestaurantOfferDto> {
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
                    }
                }
            });

            var resourceCollection = new Resource<IEnumerable<Resource<RestaurantOfferDto>>> {
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
            _logger.LogError(ex, "An error occurred while retrieving restaurant offers for the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Update an existing restaurant offer.
    /// </summary>
    /// <param name="id">The ID of the restaurant offer to update.</param>
    /// <param name="request">The restaurant offer update request.</param>
    /// <returns>The updated restaurant offer object.</returns>
    /// <response code="200">Restaurant offer updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Restaurant offer not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Resource<RestaurantOfferDto>>>
        Update(Guid id, RestaurantOfferUpdateRequest request) {
        try {
            var updatedOffer = await _restaurantOfferService.Update(id, request);

            var resource = new Resource<RestaurantOfferDto> {
                Data = updatedOffer,
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
    /// Remove a restaurant offer by its ID.
    /// </summary>
    /// <param name="id">The ID of the restaurant offer to remove.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">Restaurant offer removed successfully.</response>
    /// <response code="404">Restaurant offer not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(Guid id) {
        try {
            await _restaurantOfferService.Delete(id);
            return NoContent();
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while deleting the restaurant offer.");
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

            var bookings = await _bookingRestaurantService.GetBookingsByAdminId(admin.UserId);

            var bookingsWithLinks = bookings.Select(booking => new Resource<BookingRestaurantDto> {
                Data = booking,
                Links = GenerateLinks(booking.Id)
            });

            var resourceCollection = new Resource<IEnumerable<Resource<BookingRestaurantDto>>> {
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