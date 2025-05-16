using ReserGo.Common.DTO;

namespace ReserGo.Common.Response;

public class BookingResponses {
    public required BookingHotelDto BookingHotel { get; set; }
    public required NotificationDto Notification { get; set; }
}