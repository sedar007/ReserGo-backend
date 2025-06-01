namespace ReserGo.Common.Entity;

public class BookingEvent : Booking {
    public Guid EventOfferId { get; set; }
    public Guid EventId { get; set; }
    public double PricePerDay { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public EventOffer EventOffer { get; set; } = null!;
    public virtual Event Event { get; set; } = null!;
}