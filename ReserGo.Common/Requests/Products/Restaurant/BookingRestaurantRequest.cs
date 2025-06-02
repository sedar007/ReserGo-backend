namespace ReserGo.Common.Requests.Products.Restaurant;

public class BookingRestaurantRequest {
    public Guid RestaurantOfferId { get; set; }
    public int NumberOfGuests { get; set; }
    public DateOnly Date { get; set; }
}