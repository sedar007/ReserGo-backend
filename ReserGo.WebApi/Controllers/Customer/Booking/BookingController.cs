using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ReserGo.Business.Interfaces;
using ReserGo.Common.Models;
using ReserGo.Common.Requests.Products.Event;
using ReserGo.Common.Response;
using ReserGo.Shared;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Attributes;
using ReserGo.WebAPI.Controllers.Administration.Products;
using ReserGo.WebAPI.Hubs;

namespace ReserGo.WebAPI.Controllers.Customer.Booking;

[ApiController]
[Tags("Booking")]
[ClientOnly]
[Route("api/customer/booking/")]
public class BookingController : ControllerBase {
    private readonly IBookingService _bookingService;
    private readonly IEventOfferService _eventOfferService;
    private readonly ILogger<BookingController> _logger;
    private readonly IHubContext<NotificationHub> _notificationHub;
    private readonly ISecurity _security;

    public BookingController(ILogger<BookingController> logger,
        ISecurity security, IBookingService bookingService,
        IHubContext<NotificationHub> notificationHub,
        IEventOfferService eventOfferService) {
        _logger = logger;
        _security = security;
        _bookingService = bookingService;
        _notificationHub = notificationHub;
        _eventOfferService = eventOfferService;
    }

    /// <summary>
    ///     Retrieve all bookings for the current user.
    /// </summary>
    /// <returns>
    ///     - **200 OK**: If the bookings are successfully retrieved.
    ///     - **401 Unauthorized**: If the user is not authenticated.
    ///     - **500 Internal Server Error**: If an unexpected error occurs.
    /// </returns>
    /// <response code="200">Bookings retrieved successfully.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="400">Invalid data provided.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("my-bookings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<BookingAllResponses>>> GetMyBookings() {
        try {
            var user = _security.GetCurrentUser();
            if (user == null) return Unauthorized();

            var bookings = await _bookingService.GetBookingsByUserId(user.UserId);


            return Ok(bookings);
        }
        catch (InvalidDataException e) {
            _logger.LogError(e, "Invalid data error while retrieving bookings");
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e) {
            _logger.LogError(e, "Unauthorized access while retrieving bookings");
            return Unauthorized();
        }
        catch (Exception e) {
            _logger.LogError(e, "An unexpected error occurred while retrieving bookings");
            return StatusCode(StatusCodes.Status500InternalServerError, Consts.UnexpectedError);
        }
    }
}