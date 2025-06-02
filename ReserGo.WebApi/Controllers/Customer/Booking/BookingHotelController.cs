using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ReserGo.Business.Interfaces;
using ReserGo.Common.Models;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Shared;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Attributes;
using ReserGo.WebAPI.Hubs;
using ReserGo.Common.Response;

namespace ReserGo.WebAPI.Controllers.Customer.Booking;

[ApiController]
[Tags("Booking | Hotel")]
[Route("api/customer/booking/hotels/")]
public class BookingHotelController : ControllerBase {
    private readonly IBookingHotelService _bookingHotelService;
    private readonly ILogger<BookingHotelController> _logger;
    private readonly IHubContext<NotificationHub> _notificationHub;
    private readonly ISecurity _security;
    private readonly IRoomAvailabilityService _roomAvailabilityService;

    public BookingHotelController(ILogger<BookingHotelController> logger,
        ISecurity security, IBookingHotelService bookingHotelService, 
        IHubContext<NotificationHub> notificationHub,
        IRoomAvailabilityService roomAvailabilityService) {
        _logger = logger;
        _security = security;
        _bookingHotelService = bookingHotelService;
        _notificationHub = notificationHub;
        _roomAvailabilityService = roomAvailabilityService;
    }

    /// <summary>
    ///     Create a new hotel booking reservation.
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
    public async Task<IActionResult> CreateReservation([FromBody] BookingHotelRequest request) {
        try {
            var user = _security.GetCurrentUser();
            if (user == null) return Unauthorized();

            var responses = await _bookingHotelService.CreateBooking(request, user);
            var notification = responses.Notification;


            await _notificationHub.Clients.User(notification.UserId.ToString())
                .SendAsync(Consts.ReceiveNotification, notification.Message);
            var bookingHotelService = responses.Bookings;


            return CreatedAtAction(nameof(CreateReservation), bookingHotelService);
        }
        catch (InvalidDataException e) {
            _logger.LogError(e, "Error creating booking hotel");
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e) {
            _logger.LogError(e, "Unauthorized access");
            return Unauthorized();
        }
        catch (Exception e) {
            _logger.LogError(e, "An unexpected error occurred while creating booking hotel");
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }
    
    /// <summary>
    ///     Searches for room availability based on the provided criteria.
    /// </summary>
    /// <param name="hotelSearchAvailabilityRequest">The search criteria including arrival date and return date.</param>
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
        [FromQuery] HotelSearchAvailabilityRequest hotelSearchAvailabilityRequest) {
        try {
            var availability = await _roomAvailabilityService.SearchAvailability(hotelSearchAvailabilityRequest);

            return Ok(availability.Select(a => new Resource<RoomAvailibilityHotelResponse> {
                Data = a,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(SearchAvailability), new {
                            hotelSearchAvailabilityRequest.ArrivalDate,
                            hotelSearchAvailabilityRequest.ReturnDate
                        }) ?? string.Empty,
                        Rel = "self",
                        Method = "GET"
                    }
                }
            }));
        }
        catch (Exception e) {
            _logger.LogError(e, "An error occurred while searching for availability.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }
    
}

