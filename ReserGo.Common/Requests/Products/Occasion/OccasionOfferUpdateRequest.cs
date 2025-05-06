namespace ReserGo.Common.Requests.Products.Occasion;

public class OccasionOfferUpdateRequest {
    public String OfferTitle { get; init; } = null!;
    public String? Description { get; init; }
    public double Price { get; init; }
    public int NumberOfGuests { get; init; }
    public DateTime OfferStartDate { get; init; }
    public DateTime OfferEndDate { get; init; }
    public bool IsActive { get; init; }
   
}