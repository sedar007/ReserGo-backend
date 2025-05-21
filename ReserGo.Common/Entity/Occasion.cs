namespace ReserGo.Common.Entity;

public class Occasion {
    public Guid Id { get; init; }
    public long StayId { get; init; }
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int Capacity { get; set; }
    public string? Picture { get; set; }
    public IEnumerable<BookingOccasion> BookingsOccasion { get; init; } = null!;
    public IEnumerable<OccasionOffer> OccasionOffers { get; init; } = null!;
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}