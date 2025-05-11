namespace ReserGo.Common.Entity;

public class BookingHotel {
    public Guid Id { get; init; }
    public Guid HotelOfferId { get; set; }
    public HotelOffer HotelOffer { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime BookingDate { get; set; }
    public int NumberOfGuests { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
}
