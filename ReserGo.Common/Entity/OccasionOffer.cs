namespace ReserGo.Common.Entity;

public class OccasionOffer {
    public int Id { get; set; }
    public string OfferTitle { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }
    public int NumberOfGuests { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public bool IsActive { get; set; }
    public int OccasionId { get; set; }
    public Occasion Occasion { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
