namespace ReserGo.Common.Entity;

public class BookingOccasion {
    public Guid Id { get; init; }
    public Guid UserId { get; set; }
    public Guid OccasionId { get; set; }

    public DateTime BookingDate { get; set; }
    public string Status { get; set; } = null!;
    public bool VipAccess { get; set; } = false;

    public User User { get; set; } = null!;
    public Occasion Occasion { get; set; } = null!;

    public DateTime LastUpdated { get; set; } = DateTime.Now;
}