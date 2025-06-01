using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Attributes;
using ReserGo.WebAPI.Controllers.Administration.Products;

namespace ReserGo.WebAPI.Controllers.Administration.Notification;

[ApiController]
[Tags("Notifications")]
[AdminOnly]
[Route("api/administration/notifications/")]
public class NotificationController : ControllerBase {
    private readonly ILogger<HotelController> _logger;
    private readonly INotificationService _notificationService;
    private readonly ISecurity _security;

    public NotificationController(ILogger<HotelController> logger, INotificationService notificationService,
        ISecurity security) {
        _logger = logger;
        _notificationService = notificationService;
        _security = security;
    }

    [HttpGet("latest/{count}")]
    [ProducesResponseType(typeof(IEnumerable<NotificationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLatestNotifications(int count = 10) {
        try {
            var user = _security.GetCurrentUser();
            if (user == null) return Unauthorized();
            if (count <= 0) return BadRequest("Count must be greater than 0");
            var notifications = await _notificationService.GetLatestNotifications(user.UserId, count);
            return Ok(notifications);
        }
        catch (Exception e) {
            _logger.LogError(e, "Error getting latest notifications");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error getting latest notifications");
        }
    }

    //read notification
    [HttpPut("read/{notificationId}")]
    [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ReadNotification(Guid notificationId) {
        try {
            var user = _security.GetCurrentUser();
            if (user == null) return Unauthorized();
            var notification = await _notificationService.ReadNotification(notificationId);
            if (notification == null) return NotFound();
            return Ok(notification);
        }
        catch (Exception e) {
            _logger.LogError(e, "Error reading notification");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error reading notification");
        }
    }
}