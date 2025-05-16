namespace ReserGo.Common.Requests.Products.Hotel;

public class BookingHotelRequest {
    public Guid HotelOfferId { get; set; }
    public int NumberOfGuests { get; set; }
    public DateTime BookingDate { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}