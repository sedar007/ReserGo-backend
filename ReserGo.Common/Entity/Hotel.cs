namespace ReserGo.Common.Entity;
public class Hotel {
    public int Id { get; set; }
    public long StayId { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public int Capacity { get; set; }
    public IEnumerable<BookingHotel> BookingsHotel { get; set; }
    public IEnumerable<HotelOffer> HotelOffers { get; set; }
    public string? Picture { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime? LastUpdated { get; set; }
}