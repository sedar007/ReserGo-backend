namespace ReserGo.Common.Entity;

public abstract class Offer {
    public Guid Id { get; init; }
    public string? Description { get; set; } = null!;
    public int GuestNumber { get; set; } = 0;
    public DateOnly OfferStartDate { get; set; }
    public DateOnly OfferEndDate { get; set; }
    public bool IsActive { get; set; }
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
}