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

public class BookingAllResponses {
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string ImageSrc { get; init; }
    public required int NbGuest { get; init; }
    public required double TotalPrice { get; init; }
    public required DateOnly StartDate { get; init; }
    public required DateOnly EndDate { get; init; }
}