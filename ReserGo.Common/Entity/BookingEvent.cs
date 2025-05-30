namespace ReserGo.Common.Entity;

public class BookingEvent : Booking {
    public Guid EventId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public virtual Event Event { get; set; } = null!;
}