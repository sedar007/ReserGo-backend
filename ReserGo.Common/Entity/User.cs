using ReserGo.Common.Enum;

namespace ReserGo.Common.Entity;

public class User {
    public Guid Id { get; init; } = Guid.NewGuid();
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Bio { get; set; }
    public string Username { get; set; } = null!;
    public int? AddressId { get; init; }
    public string? ProfilePicture { get; set; }
    public UserRole Role { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public IEnumerable<BookingHotel> BookingsHotel { get; init; } = null!;
    public IEnumerable<BookingOccasion> BookingsOccasion { get; init; } = null!;
    public IEnumerable<BookingRestaurant> BookingsRestaurant { get; init; } = null!;

    // Relation avec Login (One-to-One)
    public virtual Login Login { get; init; } = null!;

    // Relation with Address (One-to-One)
    public virtual Address? Address { get; set; }

    public virtual ICollection<Hotel> Hotels { get; init; } = new List<Hotel>();
    public virtual ICollection<Restaurant> Restaurants { get; init; } = new List<Restaurant>();
    public virtual ICollection<Occasion> Occasions { get; init; } = new List<Occasion>();
    public virtual ICollection<HotelOffer> HotelOffers { get; init; } = new List<HotelOffer>();
    public virtual ICollection<RestaurantOffer> RestaurantOffers { get; init; } = new List<RestaurantOffer>();
    public virtual ICollection<OccasionOffer> OccasionOffers { get; init; } = new List<OccasionOffer>();
}