namespace ReserGo.Common.Entity;

public class Hotel {
    public Guid Id { get; init; }
    public long StayId { get; set; }
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int NumberOfRooms { get; set; } = 0;
    public string? Description { get; set; }
    public string? Picture { get; set; }
    public IEnumerable<Room> Rooms { get; init; } = null!;
    public IEnumerable<BookingHotel> BookingsHotel { get; init; } = null!;
    public IEnumerable<HotelOffer> HotelOffers { get; init; } = null!;
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}