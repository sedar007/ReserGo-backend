using ReserGo.Common.Enum;
namespace ReserGo.Common.DTO;
public class UserDto {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public IEnumerable<BookingHotelDto> BookingsHotel { get; set; }
    public IEnumerable<BookingOccasionDto> BookingsOccasion { get; set; }
    public IEnumerable<BookingRestaurantDto> BookingsRestaurant { get; set; }
    
    // Relation avec Login
    public virtual LoginDto Login { get; set; } = null!;
}