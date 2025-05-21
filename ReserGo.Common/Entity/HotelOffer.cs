namespace ReserGo.Common.Entity;

public class HotelOffer {
    public Guid Id { get; init; }
    public string OfferTitle { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double PricePerNight { get; set; }
    public int NumberOfGuests { get; set; }
    public int NumberOfRooms { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public bool IsActive { get; set; }

    public Guid HotelId { get; init; }
    public Hotel Hotel { get; init; } = null!;

    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
    public ICollection<BookingHotel> Bookings { get; set; } = new List<BookingHotel>();
}