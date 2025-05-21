namespace ReserGo.Common.DTO;
public class BookingRestaurantDto : BookingDto{
    public Guid RestaurantOfferId { get; set; }
    public RestaurantOfferDto? RestaurantOffer { get; set; }
}