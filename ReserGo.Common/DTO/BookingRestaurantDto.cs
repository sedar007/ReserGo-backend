namespace ReserGo.Common.DTO;

public class BookingRestaurantDto : BookingDto {
    public Guid RestaurantId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid RestaurantOfferId { get; set; }
    public RestaurantOfferDto? RestaurantOffer { get; set; }

    public virtual RestaurantDto Restaurant { get; set; } = null!;
}