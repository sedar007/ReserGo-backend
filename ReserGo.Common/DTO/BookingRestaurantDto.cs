namespace ReserGo.Common.DTO;
public class BookingRestaurantDto {
    public Guid Id { get; init; }
    public Guid RestaurantOfferId { get; set; }
    public RestaurantOfferDto RestaurantOffer { get; set; } = null!;
    public Guid UserId { get; set; }
    public UserDto User { get; set; } = null!;
    public DateTime BookingDate { get; set; }
    public int NumberOfGuests { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
}