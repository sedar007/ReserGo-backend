using ReserGo.Common.Enum;
namespace ReserGo.Common.Entity;
public class User {
    public int Id { get; set; }
    public String FirstName { get; set; } = null!;
    public String LastName { get; set; } = null!;
    public String Email { get; set; } = null!;
    public String? PhoneNumber { get; set; }
    public String? Bio { get; set; }
    public String Username { get; set; } = null!;
    public int? AddressId { get; set; }
    public String? ProfilePicture { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public IEnumerable<BookingHotel> BookingsHotel { get; set; } = null!;
    public IEnumerable<BookingOccasion> BookingsOccasion { get; set; } = null!;
    public IEnumerable<BookingRestaurant> BookingsRestaurant { get; set; } = null!;
    
    // Relation avec Login (One-to-One)
    public virtual Login Login { get; set; } = null!;
    // Relation with Address (One-to-One)
    public virtual Address? Address { get; set; } = null!;
    
    public virtual ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
    public virtual ICollection<Occasion> Occasions { get; set; } = new List<Occasion>();
    public virtual ICollection<HotelOffer> HotelOffers { get; set; } = new List<HotelOffer>();
    public virtual ICollection<RestaurantOffer> RestaurantOffers { get; set; } = new List<RestaurantOffer>();
    public virtual ICollection<OccasionOffer> OccasionOffers { get; set; } = new List<OccasionOffer>();
    
}