using ReserGo.Common.Enum;

namespace ReserGo.Common.Entity;

public class User {
    public Guid Id { get; init; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Bio { get; set; }

    public string Username { get; set; } = null!;
    public string? ProfilePicture { get; set; }
    public UserRole Role { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public IEnumerable<BookingHotel> BookingsHotel { get; init; } = null!;
    public IEnumerable<BookingEvent> BookingsEvent { get; init; } = null!;
    public IEnumerable<BookingRestaurant> BookingsRestaurant { get; init; } = null!;

    // Relation avec Login (One-to-One)
    public virtual Login Login { get; init; } = null!;

    // Relation with Address (One-to-One)
    public virtual Address? Address { get; set; }
    public ICollection<Notification> Notifications { get; init; } = new List<Notification>();
    public virtual ICollection<Hotel> Hotels { get; init; } = new List<Hotel>();
    public virtual ICollection<Restaurant> Restaurants { get; init; } = new List<Restaurant>();
    public virtual ICollection<Event> Events { get; init; } = new List<Event>();
    public virtual ICollection<HotelOffer> HotelOffers { get; init; } = new List<HotelOffer>();
    public virtual ICollection<RestaurantOffer> RestaurantOffers { get; init; } = new List<RestaurantOffer>();
    public virtual ICollection<EventOffer> EventOffers { get; init; } = new List<EventOffer>();
}