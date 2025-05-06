namespace ReserGo.Common.Entity;

public class Restaurant {
    public int Id { get; set; }
    public long StayId { get; set; }
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string CuisineType { get; set; } = null!;
    public int Capacity { get; set; }
    public string? Picture { get; set; }
    public IEnumerable<BookingRestaurant> BookingRestaurant { get; set; } = null!;
    public IEnumerable<RestaurantOffer> RestaurantOffers { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}