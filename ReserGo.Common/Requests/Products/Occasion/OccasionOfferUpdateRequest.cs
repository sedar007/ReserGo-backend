namespace ReserGo.Common.Requests.Products.Occasion;

public class OccasionOfferUpdateRequest {
    public string? Description { get; init; }
    public double PricePerPerson { get; init; }
    public int GuestLimit { get; init; }
    public DateTime OfferStartDate { get; init; }
    public DateTime OfferEndDate { get; init; }
    public bool IsActive { get; init; }
}