namespace ReserGo.Common.Requests.Products.Restaurant;

public class BookingRestaurantRequest {
    public Guid RestaurantOfferId { get; set; }
    public int NumberOfGuests { get; set; }
    public DateTime BookingDate { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}