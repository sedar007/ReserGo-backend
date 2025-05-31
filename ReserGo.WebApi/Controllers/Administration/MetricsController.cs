using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.WebAPI.Attributes;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Controllers.Administration.Products;
using Microsoft.AspNetCore.SignalR;
using ReserGo.WebAPI.Hubs;
using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.WebAPI.Attributes;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Controllers.Administration.Products;
using ReserGo.Common.Models;
using ReserGo.Common.Response;
using ReserGo.Common.Enum;

namespace ReserGo.WebAPI.Controllers.Administration;

[ApiController]
[Tags("Metrics")]
[AdminOnly]
[Route("api/administration/metrics/")]
public class MetricsController : ControllerBase {
    private readonly ILogger<HotelController> _logger;
    private readonly ISecurity _security;
    private readonly IBookingHotelService _bookingHotelService;
    private readonly IHubContext<NotificationHub> _notificationHub;
    private readonly IMetricsService _metricsService;

    public MetricsController(ILogger<HotelController> logger,
        ISecurity security, IBookingHotelService bookingHotelService, IHubContext<NotificationHub> notificationHub,
        IMetricsService metricsService) {
        _logger = logger;
        _security = security;
        _bookingHotelService = bookingHotelService;
        _notificationHub = notificationHub;
        _metricsService = metricsService;
    }


    [HttpGet("months/{types}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MetricsResponse>> GetMetricsMonths(Product product) {
        try {
            var user = _security.GetCurrentUser();
            if (user == null) return Unauthorized();

            var responses = await _metricsService.GetMetricsMonths(product, user.UserId);


            return Ok(responses);
        }
        catch (Exception e) {
            _logger.LogError(e, "An unexpected error occurred while retrieving bookings");
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }
    [HttpGet("months")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Dictionary<string, double>>> GetMonthlySales() {
        try {
            var user = _security.GetCurrentUser();
            if (user == null) return Unauthorized();

            var responses = await _metricsService.GetMonthlySales(user.UserId);


            return Ok(responses);
        }
        catch (Exception e) {
            _logger.LogError(e, "An unexpected error occurred while retrieving bookings");
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }
    
    /// <summary>
    /// Get the percentage change in bookings over the last 30 days compared to the previous 30 days.
    /// </summary>
    /// <returns>
    /// A response containing the number of bookings in the last 30 days, 
    /// the percentage change, and whether the trend is upward or downward.
    /// </returns>
    /// <response code="200">Metrics retrieved successfully.</response>
    /// <response code="401">User not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("{type}/nbBookingsLast30Days")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MetricsResponse>> GetBookingMetrics(Product type) {
        try {
            var admin = _security.GetCurrentUser();
            if (admin == null) return Unauthorized("User not authenticated.");

            var metrics = await _metricsService.GetNbBookingsLast30Days(admin.UserId, type);
            return Ok(metrics);
        }
        catch (InvalidDataException ex) {
            _logger.LogError(ex, "Invalid data provided for booking metrics.");
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving booking metrics.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }
    
    /// <summary>
    /// Get the monthly bookings grouped by category (Hotel, Restaurant, Event).
    /// </summary>
    /// <returns>
    /// A dictionary where each key is a category and the value is another dictionary containing months and booking counts.
    /// </returns>
    /// <response code="200">Monthly bookings by category retrieved successfully.</response>
    /// <response code="401">User not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("monthly-bookings-by-category")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMonthlyBookingsByCategory() {
        try {
            var user = _security.GetCurrentUser();
            if (user == null) return Unauthorized("User not authenticated.");

            var result = await _metricsService.GetMonthlyBookingsByCategory(user.UserId);
            return Ok(result);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving monthly bookings by category.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }
    
    
    
}