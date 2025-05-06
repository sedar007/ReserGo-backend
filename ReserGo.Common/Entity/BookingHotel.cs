namespace ReserGo.Common.Entity;

public class BookingHotel {
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid HotelId { get; set; }
    public DateTime BookingDate { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NumberOfGuests { get; set; }
    public Hotel Hotel { get; set; } = null!;
    public User User { get; set; } = null!;

    public DateTime? LastUpdated { get; set; } = DateTime.Now;
}