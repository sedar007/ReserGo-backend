namespace ReserGo.Common.Entity;

public class Hotel {
    public Guid Id { get; init; }
    public long StayId { get; init; }
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int Capacity { get; set; }
    public IEnumerable<BookingHotel> BookingsHotel { get; init; } = null!;
    public IEnumerable<HotelOffer> HotelOffers { get; init; } = null!;
    public string? Picture { get; set; }
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}