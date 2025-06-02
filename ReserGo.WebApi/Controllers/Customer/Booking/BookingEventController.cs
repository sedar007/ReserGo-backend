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
[Tags("Booking | Event")]
[Route("api/customer/booking/events/")]
public class BookingEventController : ControllerBase {
    private readonly IBookingEventService _bookingEventService;
    private readonly IEventOfferService _eventOfferService;
    private readonly ILogger<BookingEventController> _logger;
    private readonly IHubContext<NotificationHub> _notificationHub;
    private readonly ISecurity _security;

    public BookingEventController(ILogger<BookingEventController> logger,
        ISecurity security, IBookingEventService bookingEventService,
        IHubContext<NotificationHub> notificationHub,
        IEventOfferService eventOfferService) {
        _logger = logger;
        _security = security;
        _bookingEventService = bookingEventService;
        _notificationHub = notificationHub;
        _eventOfferService = eventOfferService;
    }

    /// <summary>
    ///     Create a new event booking reservation.
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
    public async Task<IActionResult> CreateReservation([FromBody] BookingEventRequest request) {
        var user = _security.GetCurrentUser();
        if (user == null) return Unauthorized();
        try {
            var responses = await _bookingEventService.CreateBooking(request, user);
            var notification = responses.Notification;

            await _notificationHub.Clients.User(notification.UserId.ToString())
                .SendAsync(Consts.ReceiveNotification, notification.Message);
            var bookingEventService = responses.Booking;

            return CreatedAtAction(nameof(CreateReservation), bookingEventService);
        }
        catch (InvalidDataException e) {
            _logger.LogError(e, "Error creating booking event");
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e) {
            _logger.LogError(e, "Unauthorized access");
            return Unauthorized();
        }
        catch (Exception e) {
            _logger.LogError(e, "An unexpected error occurred while creating booking event");
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }

    /// <summary>
    ///     Searches for event availability based on the provided criteria.
    /// </summary>
    /// <param name="eventSearchAvailabilityRequest">The search criteria including date and number of guests.</param>
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
        [FromQuery] EventSearchAvailabilityRequest eventSearchAvailabilityRequest) {
        try {
            var availabilityList = await _eventOfferService.SearchAvailability(eventSearchAvailabilityRequest);

            return Ok(availabilityList.Select(a => new Resource<EventAvailabilityResponse> {
                Data = a,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(SearchAvailability), new {
                            eventSearchAvailabilityRequest.StartDate,
                            eventSearchAvailabilityRequest.EndDate,
                            eventSearchAvailabilityRequest.NumberOfGuests
                        }) ?? throw new InvalidOperationException("Failed to generate URL for SearchAvailability."),
                        Rel = "self",
                        Method = "GET"
                    }
                }
            }));
        }
        catch (InvalidDataException e) {
            _logger.LogError(e, "Invalid data provided for event availability search.");
            return BadRequest(e.Message);
        }

        catch (Exception e) {
            _logger.LogError(e, "An error occurred while searching for event availability.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }
}