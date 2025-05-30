namespace ReserGo.Common.Requests.Products.Hotel;

public class BookingHotelRequest {
    public Guid RoomId { get; set; }
    public Guid HotelId { get; set; }
    public Guid UserId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int NumberOfGuests { get; set; }
    public bool IsConfirmed { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}