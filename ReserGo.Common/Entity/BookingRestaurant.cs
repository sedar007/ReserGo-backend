namespace ReserGo.Common.Entity;
public class BookingRestaurant {
    public Guid Id { get; init; }
    public Guid RestaurantOfferId { get; set; }
    public RestaurantOffer RestaurantOffer { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime BookingDate { get; set; }
    public int NumberOfGuests { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
}