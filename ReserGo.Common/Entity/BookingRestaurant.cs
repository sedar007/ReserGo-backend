namespace ReserGo.Common.Entity;
public class BookingRestaurant : Booking {
    public Guid RestaurantOfferId { get; set; }
    public Guid RestaurantId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public RestaurantOffer RestaurantOffer { get; set; } = null!;
    public virtual Restaurant Restaurant { get; set; } = null!;
}