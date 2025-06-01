namespace ReserGo.Common.Requests.Products.Hotel.Rooms;

public class RoomAvailabilityRequest {
    public Guid HotelId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}