namespace ReserGo.Common.Entity;
public class Occasion {
    public int Id { get; set; }
    public long StayId { get; set; }
    public String Name { get; set; } = null!;
    public String Location { get; set; } = null!;
    public int Capacity { get; set; } 
    public String? Picture { get; set; }
    public IEnumerable<BookingOccasion> BookingsOccasion { get; set; } = null!;
    public IEnumerable<OccasionOffer> OccasionOffers { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}