namespace ReserGo.Common.Entity;

public class EventOffer {
    public Guid Id { get; init; }
    public string? Description { get; set; } = null!;
    public double PricePerDay { get; set; }
    public int GuestNumber { get; set; } = 0;
    public int GuestLimit { get; set; }
    public DateOnly OfferStartDate { get; set; }
    public DateOnly OfferEndDate { get; set; }
    public bool IsActive { get; set; }
    public Guid EventId { get; init; }
    public Event Event { get; init; } = null!;
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
    public ICollection<BookingEvent> Bookings { get; set; } = new List<BookingEvent>();
}