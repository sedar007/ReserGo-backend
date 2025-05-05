namespace ReserGo.Common.Entity;
public class Occasion {
    public int Id { get; set; }
    public long StayId { get; set; }
    public string Name { get; set; }
    public string Location { get; set; } 
    public int Capacity { get; set; } 
    public string? Picture { get; set; }
    public IEnumerable<BookingOccasion> BookingsOccasion { get; set; }
    public IEnumerable<OccasionOffer> OccasionOffers { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime? LastUpdated { get; set; }
}