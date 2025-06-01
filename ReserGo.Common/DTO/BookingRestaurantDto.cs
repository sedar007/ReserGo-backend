namespace ReserGo.Common.DTO;

public class BookingRestaurantDto : BookingDto {
    public Guid RestaurantId { get; set; }
    public DateOnly Date { get; set; }
    public double PricePerPerson { get; set; }

    public Guid RestaurantOfferId { get; set; }
    public RestaurantOfferDto? RestaurantOffer { get; set; }

    public virtual RestaurantDto Restaurant { get; set; } = null!;
}