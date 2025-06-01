using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.Requests.Products.Event;
using ReserGo.WebAPI.Attributes;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Controllers.Administration.Products;
using Microsoft.AspNetCore.SignalR;
using ReserGo.WebAPI.Hubs;
using ReserGo.Common.DTO;
using ReserGo.Common.Models;
using ReserGo.Common.Response;
namespace ReserGo.WebAPI.Controllers.Customer.Booking;

[ApiController]
[Tags("Booking | Event")]
[Route("api/customer/booking/events/")]
public class BookingEventController : ControllerBase {
    private readonly ILogger<EventController> _logger;
    private readonly ISecurity _security;
    private readonly IBookingEventService _bookingEventService;
    private readonly IHubContext<NotificationHub> _notificationHub;
    private readonly IEventOfferService _eventOfferService;

    public BookingEventController(ILogger<EventController> logger,
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
        try {
            var user = _security.GetCurrentUser();
            if (user == null) return Unauthorized();

            var responses = await _bookingEventService.CreateBooking(request, user);
            var notification = responses.Notification;

            await _notificationHub.Clients.User(notification.UserId.ToString())
                .SendAsync("ReceiveNotification", notification.Message);
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
    ///     Retrieve all bookings for the current user.
    /// </summary>
    /// <returns>
    ///     - **200 OK**: If the bookings are successfully retrieved.
    ///     - **401 Unauthorized**: If the user is not authenticated.
    ///     - **500 Internal Server Error**: If an unexpected error occurs.
    /// </returns>
    /// <response code="200">Bookings retrieved successfully.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [ClientOnly]
    [HttpGet("my-bookings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<IEnumerable<Resource<BookingEventDto>>>>> GetMyBookings() {
        try {
            var user = _security.GetCurrentUser();
            if (user == null) return Unauthorized();

           /* var bookings = await _bookingEventService.GetBookingsByUserId(user.UserId);

            var bookingsWithLinks = bookings.Select(booking => new Resource<BookingEventDto> {
                Data = booking,
                Links = GenerateLinks(booking.Id)
            });

            var resourceCollection = new Resource<IEnumerable<Resource<BookingEventDto>>> {
                Data = bookingsWithLinks,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetMyBookings)),
                        Rel = "self",
                        Method = "GET"
                    }
                }
            };

            return Ok(resourceCollection);*/
           return Ok(1);
        }
        catch (Exception e) {
            _logger.LogError(e, "An unexpected error occurred while retrieving bookings");
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }
    
    /// <summary>
    /// Searches for event availability based on the provided criteria.
    /// </summary>
    /// <param name="eventSearchAvailabilityRequest">The search criteria including date and number of guests.</param>
    /// <returns>
    /// - **200 OK**: If availability is found.
    /// - **400 Bad Request**: If the request is invalid.
    /// - **500 Internal Server Error**: If an unexpected error occurs.
    /// </returns>
    /// <response code="200">Availability found and returned.</response>
    /// <response code="400">Invalid search criteria.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("search-availability")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchAvailability([FromQuery] EventSearchAvailabilityRequest eventSearchAvailabilityRequest)
    {
        try { 
            var availability = await _eventOfferService.SearchAvailability(eventSearchAvailabilityRequest);

            return Ok(availability.Select(a => new Resource<EventAvailabilityResponse> {
                Data = a,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(SearchAvailability), new {
                            eventSearchAvailabilityRequest.StartDate,
                            eventSearchAvailabilityRequest.EndDate,
                            eventSearchAvailabilityRequest.NumberOfGuests
                        }),
                        Rel = "self",
                        Method = "GET"
                    }
                }
            }));
        } catch (Exception e) {
            _logger.LogError(e, "An error occurred while searching for event availability.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }

    private List<Link> GenerateLinks(Guid bookingId) {
        return new List<Link> {
            new() {
                Href = Url.Action(nameof(GetMyBookings), new { id = bookingId }),
                Rel = "self",
                Method = "GET"
            },
            new() {
                Href = Url.Action(nameof(CreateReservation), new { id = bookingId }),
                Rel = "create",
                Method = "POST"
            },
            new() {
                Href = Url.Action("DeleteBooking", new { id = bookingId }),
                Rel = "delete",
                Method = "DELETE"
            }
        };
    }
}