namespace ReserGo.Common.Requests.Products.Hotel;

public class BookingHotelRequest {
    public IEnumerable<RoomData> Rooms { get; set; } = new List<RoomData>();
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; set; }
}

public class RoomData {
    public Guid RoomId { get; set; }
    public int NumberOfGuests { get; set; }
}