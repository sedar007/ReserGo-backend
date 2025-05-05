namespace ReserGo.Common.Requests.Products.Restaurant;

public class RestaurantOfferUpdateRequest {
    public String OfferTitle { get; init; } = null!;
    public String? Description { get; init; }
    public double? PricePerPerson { get; init; }
    public int NumberOfGuests { get; init; }
    public DateTime OfferStartDate { get; init; }
    public DateTime OfferEndDate { get; init; }
    public bool IsActive { get; init; }
   
}