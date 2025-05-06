using ReserGo.Common.Enum;
namespace ReserGo.Common.DTO;
public class UserDto {
    public int Id { get; set; }
    public String FirstName { get; set; } = null!;
    public String LastName { get; set; } = null!;
    public String Username { get; set; } = null!;
    public String? PhoneNumber { get; set; }
    public String? Bio { get; set; }
    public String Email { get; set; } = null!;
    public UserRole Role { get; set; }
    public String? RoleString { get; set; }
    public String? ProfilePicture { get; set; }
    public IEnumerable<BookingHotelDto> BookingsHotel { get; set; } = null!;
    public IEnumerable<BookingOccasionDto> BookingsOccasion { get; set; } = null!;
    public IEnumerable<BookingRestaurantDto> BookingsRestaurant { get; set; } = null!;
    
    // Relation avec Login
    public virtual LoginDto Login { get; set; } = null!;
    // Relation with Address
    public virtual AddressDto? Address { get; set; }
}