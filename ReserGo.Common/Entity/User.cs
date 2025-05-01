using ReserGo.Common.Enum;
namespace ReserGo.Common.Entity;
public class User {
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Bio { get; set; }
    public string Username { get; set; } = null!;
    public int? AddressId { get; set; }
    public string? ProfilePicture { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public IEnumerable<BookingHotel> BookingsHotel { get; set; } = null!;
    public IEnumerable<BookingOccasion> BookingsOccasion { get; set; } = null!;
    public IEnumerable<BookingRestaurant> BookingsRestaurant { get; set; } = null!;
    
    // Relation avec Login (One-to-One)
    public virtual Login Login { get; set; } = null!;
    // Relation with Address (One-to-One)
    public virtual Address? Address { get; set; } = null!;
}