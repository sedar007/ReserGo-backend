using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;
using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Enum;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.User;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;
using ReserGo.Common.Requests.Notification;

namespace ReserGo.Business.Implementations;

public class NotificationService : INotificationService {
    private readonly ILogger<UserService> _logger;
    private readonly ILoginService _loginService;
    private readonly IUserDataAccess _userDataAccess;
    private readonly IImageService _imageService;
    private readonly IMemoryCache _cache;
    private readonly INotificationDataAccess _notificationDataAccess;

    public NotificationService(ILogger<UserService> logger, IUserDataAccess userDataAccess,
        ILoginService loginService, IImageService imageService, IMemoryCache cache, INotificationDataAccess notificationService) {
        _logger = logger;
        _loginService = loginService;
        _userDataAccess = userDataAccess;
        _imageService = imageService;
        _cache = cache;
        _notificationDataAccess = notificationService;
    }
    
    public async Task<NotificationDto> CreateNotification(NotificationCreationRequest request) {
        try {
            if (request == null) {
                _logger.LogError("Request is null");
                throw new InvalidDataException("Request is null");
            }
            var notification = new Notification {
                UserId = request.UserId,
                Message = request.Message,
                Title = request.Title,
                HotelName = request.HotelName,
                Type = request.Type,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            _logger.LogInformation("Creating notification for user { id }", request.UserId);
            notification = await _notificationDataAccess.Create(notification);
            
            if (notification == null) {
                throw new InvalidDataException("Notification not created");
            }
            return notification.ToDto();
        }
        catch (Exception e) {
            _logger.LogError(e, "Error creating notification");
            throw;
        }
    }
    
    public async Task<IEnumerable<NotificationDto>> GetLatestNotifications(Guid userId, int count) {
        try {
            var notifications = await _notificationDataAccess.GetLatestNotifications(userId, count);
            return notifications.Select(n => n.ToDto());
        }
        catch (Exception e) {
            _logger.LogError(e, "Error getting latest notifications");
            throw;
        }
    }
    public async Task<NotificationDto> ReadNotification(Guid notificationId) {
        try {
            var notification = await _notificationDataAccess.GetById(notificationId);
            if (notification == null) {
                throw new InvalidDataException("Notification not found");
            }
            notification.IsRead = true;
            await _notificationDataAccess.Update(notification);
            return notification.ToDto();
        }
        catch (Exception e) {
            _logger.LogError(e, "Error reading notification");
            throw;
        }
    }

    
}