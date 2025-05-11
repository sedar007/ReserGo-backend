using Microsoft.AspNetCore.Http;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.User;
using ReserGo.Common.Enum;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Requests.Notification;

namespace ReserGo.Business.Interfaces;

public interface INotificationService {
    Task<NotificationDto> CreateNotification(NotificationCreationRequest request);
    Task<IEnumerable<NotificationDto>> GetLatestNotifications(Guid userId, int count);
    // read notification 
    Task<NotificationDto> ReadNotification(Guid notificationId);
    //
   
    
}