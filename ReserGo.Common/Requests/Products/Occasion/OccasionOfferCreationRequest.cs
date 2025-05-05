namespace ReserGo.Common.Requests.Products.Occasion;

public class OccasionOfferCreationRequest {
    public string OfferTitle { get; set; } = null!;
    public string? Description { get; set; }
    public double Price { get; set; }
    public int NumberOfGuests { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public bool IsActive { get; set; }
    public int OccasionId { get; set; }
}