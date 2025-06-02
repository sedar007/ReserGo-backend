using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Notification;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.Business.Implementations;

public class NotificationService : INotificationService {
    private readonly ILogger<NotificationService> _logger;
    private readonly INotificationDataAccess _notificationDataAccess;

    public NotificationService(ILogger<NotificationService> logger,
        INotificationDataAccess notificationService) {
        _logger = logger;
       
        _notificationDataAccess = notificationService;
    }

    public async Task<NotificationDto> CreateNotification(NotificationCreationRequest request) {
      
            if (request == null) {
                _logger.LogError("Request is null");
                throw new InvalidDataException("Request is null");
            }

            var notification = new Notification {
                UserId = request.UserId,
                Message = request.Message,
                Title = request.Title,
                Name = request.Name,
                Type = request.Type,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            _logger.LogInformation("Creating notification for user {Id}", request.UserId);
            notification = await _notificationDataAccess.Create(notification);

            if (notification == null) throw new InvalidDataException("Notification not created");
            return notification.ToDto();
        
    }

    public async Task<IEnumerable<NotificationDto>> GetLatestNotifications(Guid userId, int count) {
       
            var notifications = await _notificationDataAccess.GetLatestNotifications(userId, count);
            return notifications.Select(n => n.ToDto());
        
    }

    public async Task<NotificationDto> ReadNotification(Guid notificationId) {
       
            var notification = await _notificationDataAccess.GetById(notificationId);
            if (notification == null) throw new InvalidDataException("Notification not found");
            notification.IsRead = true;
            await _notificationDataAccess.Update(notification);
            return notification.ToDto();
        
    }
}