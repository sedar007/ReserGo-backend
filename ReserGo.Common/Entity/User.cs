using ReserGo.Common.Enum;
namespace ReserGo.Common.Entity;
public class User {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; } = null!;
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public IEnumerable<BookingHotel> BookingsHotel { get; set; }
    public IEnumerable<BookingOccasion> BookingsOccasion { get; set; }
    public IEnumerable<BookingRestaurant> BookingsRestaurant { get; set; }
    
    // Relation avec Login (One-to-One)
    public virtual Login Login { get; set; } = null!;
}