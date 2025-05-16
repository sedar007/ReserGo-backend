using ReserGo.Common.DTO;

namespace ReserGo.Common.Response;

public class BookingResponses {
    public BookingDto Booking { get; set; }
    public required NotificationDto Notification { get; set; }
}