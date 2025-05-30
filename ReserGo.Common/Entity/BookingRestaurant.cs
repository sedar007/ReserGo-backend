namespace ReserGo.Common.Entity;

public class BookingRestaurant : Booking {
    public Guid RestaurantOfferId { get; set; }
    public Guid RestaurantId { get; set; }
    public DateOnly Date { get; set; }
    public RestaurantOffer RestaurantOffer { get; set; } = null!;
    public virtual Restaurant Restaurant { get; set; } = null!;
}