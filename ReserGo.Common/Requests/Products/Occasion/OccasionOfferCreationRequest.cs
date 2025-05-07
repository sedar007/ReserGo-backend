namespace ReserGo.Common.Requests.Products.Occasion;

public class OccasionOfferCreationRequest {
    public string OfferTitle { get; init; } = null!;
    public string? Description { get; init; }
    public double Price { get; init; }
    public int NumberOfGuests { get; init; }
    public DateTime OfferStartDate { get; init; }
    public DateTime OfferEndDate { get; init; }
    public bool IsActive { get; init; }
    public Guid OccasionId { get; init; }
}