namespace ReserGo.Common.Entity;
public class BookingRestaurant : Booking {
    public Guid RestaurantOfferId { get; set; }
    public RestaurantOffer RestaurantOffer { get; set; } = null!;
  
}