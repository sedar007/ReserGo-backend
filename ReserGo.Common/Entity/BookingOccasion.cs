namespace ReserGo.Common.Entity;

public class BookingOccasion {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int OccasionId { get; set; }

    public DateTime BookingDate { get; set; }
    public string Status { get; set; } = null!;
    public bool VipAccess { get; set; } = false;

    public User User { get; set; } = null!;
    public Occasion Occasion { get; set; } = null!;

    public DateTime LastUpdated { get; set; } = DateTime.Now;
}