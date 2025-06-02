using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Notification;

namespace ReserGo.Business.Interfaces;

public interface INotificationService {
    Task<NotificationDto> CreateNotification(NotificationCreationRequest request);
    Task<IEnumerable<NotificationDto>> GetLatestNotifications(Guid userId, int count);
    Task<NotificationDto> ReadNotification(Guid notificationId);

}