namespace ReserGo.Common.Entity;

public abstract class Booking {
    public Guid Id { get; init; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public double PriceTotal { get; set; }
    public DateOnly BookingDate { get; set; }
    public int NumberOfGuests { get; set; }
    public bool IsConfirmed { get; set; }
}