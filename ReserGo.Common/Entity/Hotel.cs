namespace ReserGo.Common.Entity;
public class Hotel {
    public int Id { get; set; }
    public long StayId { get; set; }
    public String Name { get; set; } = null!;
    public String Location { get; set; } = null!;
    public int Capacity { get; set; }
    public IEnumerable<BookingHotel> BookingsHotel { get; set; } = null!;
    public IEnumerable<HotelOffer> HotelOffers { get; set; } = null!;
    public String? Picture { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}