namespace ReserGo.Common.Response;

public class RoomAvailibilityHotelResponse {
    public Guid RoomId { get; set; }
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = null!;
    public decimal PricePerNightPerPerson { get; set; }
    public string RoomName { get; set; } = null!;
    public int NumberOfGuests { get; set; }
    public string ImageSrc { get; set; } = null!;
}