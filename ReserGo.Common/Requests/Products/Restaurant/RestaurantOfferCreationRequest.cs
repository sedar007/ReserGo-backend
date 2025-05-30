namespace ReserGo.Common.Requests.Products.Restaurant;

public class RestaurantOfferCreationRequest {
    public string? Description { get; init; } = null!;
    public double? PricePerPerson { get; init; }
    public int GuestLimit { get; init; }
    public DateOnly OfferStartDate { get; init; }
    public DateOnly OfferEndDate { get; init; }
    public bool IsActive { get; init; }
    public Guid RestaurantId { get; init; }
}