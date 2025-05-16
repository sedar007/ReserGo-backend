using ReserGo.Common.DTO;

namespace ReserGo.Common.Response;

public class BookingResponses {
    public BookingHotelDto? BookingHotel { get; set; }
    public BookingRestaurantDto? BookingRestaurant { get; set; }
    public required NotificationDto Notification { get; set; }
}