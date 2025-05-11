using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class NotificationHelper {
    public static NotificationDto ToDto(this Notification notification) {
        return new NotificationDto {
            Id = notification.Id,
            Title = notification.Title,
            Message = notification.Message,
            HotelName = notification.HotelName,
            Type = notification.Type,
            CreatedAt = notification.CreatedAt,
            IsRead = notification.IsRead,
            UserId = notification.UserId,
           // User = notification.User?.ToDto()
        };
    }
}