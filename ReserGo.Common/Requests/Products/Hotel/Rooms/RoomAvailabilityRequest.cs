namespace ReserGo.Common.Requests.Products.Hotel.Rooms;

public class RoomAvailabilitiesRequest {
    public IEnumerable<Guid> RoomIds { get; set; } = null!;
    public Guid HotelId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}


public class RoomAvailabilityRequest {
    public Guid RoomId { get; set; }
    public Guid HotelId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}