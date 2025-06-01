using ReserGo.Common.DTO;

namespace ReserGo.Common.Response;

public class BookingResponses {
    public required BookingDto Booking { get; init; }
    public required NotificationDto Notification { get; init; }
}

public class BookingHotelResponses {
    public required IEnumerable<BookingDto> Bookings { get; init; }
    public required NotificationDto Notification { get; init; }
}