namespace ReserGo.Common.Entity;

public class BookingEvent : Booking {
    public Guid EventId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public virtual Event Event { get; set; } = null!;
}