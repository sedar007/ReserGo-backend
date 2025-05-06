namespace ReserGo.Common.DTO;

public class OccasionOfferDto {
    public Guid Id { get; init; } = Guid.NewGuid();
    public string OfferTitle { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double Price { get; set; }
    public int NumberOfGuests { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public bool IsActive { get; set; }
    public Guid OccasionId { get; set; }
    public OccasionDto Occasion { get; set; } = null!;
}