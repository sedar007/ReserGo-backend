namespace ReserGo.Common.Entity;

public class BookingOccasion : Booking {
    public Guid OccasionId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public virtual Occasion Occasion { get; set; } = null!;
}

