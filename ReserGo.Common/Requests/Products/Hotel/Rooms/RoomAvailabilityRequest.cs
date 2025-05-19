namespace ReserGo.Common.Requests.Products.Hotel.Rooms;

public class RoomAvailabilityRequest {
    public Guid HotelId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}