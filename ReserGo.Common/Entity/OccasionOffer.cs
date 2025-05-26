namespace ReserGo.Common.Entity;

public class OccasionOffer {
    public Guid Id { get; init; }
    public string? Description { get; set; } = null!;
    public double PricePerPerson { get; set; }
    public int GuestLimit { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public bool IsActive { get; set; }
    public Guid OccasionId { get; init; }
    public Occasion Occasion { get; init; } = null!;
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
}