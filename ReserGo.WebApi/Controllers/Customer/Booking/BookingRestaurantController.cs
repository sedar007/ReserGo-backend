using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ReserGo.Business.Interfaces;
using ReserGo.Common.Models;
using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.Common.Response;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Attributes;
using ReserGo.WebAPI.Controllers.Administration.Products;
using ReserGo.WebAPI.Hubs;

namespace ReserGo.WebAPI.Controllers.Customer.Booking;

[ApiController]
[Tags("Booking | Restaurant")]
[Route("api/customer/booking/restaurants/")]
public class BookingRestaurantController : ControllerBase {
    private readonly IBookingRestaurantService _bookingRestaurantService;
    private readonly ILogger<RestaurantController> _logger;
    private readonly IHubContext<NotificationHub> _notificationHub;
    private readonly IRestaurantOfferService _restaurantOfferService;
    private readonly ISecurity _security;

    public BookingRestaurantController(ILogger<RestaurantController> logger,
        ISecurity security, IBookingRestaurantService bookingRestaurantService,
        IHubContext<NotificationHub> notificationHub, IRestaurantOfferService restaurantOfferService) {
        _logger = logger;
        _security = security;
        _bookingRestaurantService = bookingRestaurantService;
        _notificationHub = notificationHub;
        _restaurantOfferService = restaurantOfferService;
    }

    /// <summary>
    ///     Create a new restaurant booking reservation.
    /// </summary>
    /// <param name="request">The booking request containing necessary information.</param>
    /// <returns>
    ///     - **201 Created**: If the booking is successfully created.
    ///     - **401 Unauthorized**: If the user is not authenticated.
    ///     - **400 Bad Request**: If the booking creation fails.
    ///     - **500 Internal Server Error**: If an unexpected error occurs.
    /// </returns>
    /// <response code="201">Booking created successfully.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="400">Invalid booking request or creation failed.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [ClientOnly]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateReservation([FromBody] BookingRestaurantRequest request) {
        try {
            var user = _security.GetCurrentUser();
            if (user == null) return Unauthorized();

            var responses = await _bookingRestaurantService.CreateBooking(request, user);
            var notification = responses.Notification;

            await _notificationHub.Clients.User(notification.UserId.ToString())
                .SendAsync("ReceiveNotification", notification.Message);

            var bookingRestaurantService = responses.Booking;


            return CreatedAtAction(nameof(CreateReservation), bookingRestaurantService);
        }
        catch (InvalidDataException e) {
            _logger.LogError(e, "Error creating booking restaurant");
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e) {
            _logger.LogError(e, "Unauthorized access");
            return Unauthorized();
        }
        catch (Exception e) {
            _logger.LogError(e, "An unexpected error occurred while creating booking restaurant");
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }

    /// <summary>
    ///     Searches for restaurant availability based on the provided criteria.
    /// </summary>
    /// <param name="restaurantSearchAvailabilityRequest">The search criteria including date and number of guests.</param>
    /// <returns>
    ///     - **200 OK**: If availability is found.
    ///     - **400 Bad Request**: If the request is invalid.
    ///     - **500 Internal Server Error**: If an unexpected error occurs.
    /// </returns>
    /// <response code="200">Availability found and returned.</response>
    /// <response code="400">Invalid search criteria.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("search-availability")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchAvailability(
        [FromQuery] RestaurantSearchAvailabilityRequest restaurantSearchAvailabilityRequest) {
        try {
            var availability = await _restaurantOfferService.SearchAvailability(restaurantSearchAvailabilityRequest);

            return Ok(availability.Select(a => new Resource<RestaurantAvailabilityResponse> {
                Data = a,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(SearchAvailability), new {
                            restaurantSearchAvailabilityRequest.Date,
                            restaurantSearchAvailabilityRequest.NumberOfGuests
                        }),
                        Rel = "self",
                        Method = "GET"
                    }
                }
            }));
        }
        catch (Exception e) {
            _logger.LogError(e, "An error occurred while searching for restaurant availability.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }
}