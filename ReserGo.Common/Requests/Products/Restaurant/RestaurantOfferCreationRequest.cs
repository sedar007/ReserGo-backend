namespace ReserGo.Common.Requests.Products.Restaurant;

public class RestaurantOfferCreationRequest {
    public string OfferTitle { get; init; } = null!;
    public string Description { get; init; } = null!;
    public double? PricePerPerson { get; init; }
    public int NumberOfGuests { get; init; }
    public DateTime OfferStartDate { get; init; }
    public DateTime OfferEndDate { get; init; }
    public bool IsActive { get; init; }
    public Guid RestaurantId { get; init; }
}