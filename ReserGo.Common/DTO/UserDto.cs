using ReserGo.Common.Enum;

namespace ReserGo.Common.DTO;

public class UserDto {
    public Guid Id { get; init; } = Guid.NewGuid();
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Bio { get; set; }
    public string Email { get; set; } = null!;
    public UserRole Role { get; set; }
    public string? RoleString { get; set; }
    public string? ProfilePicture { get; set; }
    public IEnumerable<BookingHotelDto> BookingsHotel { get; set; } = null!;
    public IEnumerable<BookingOccasionDto> BookingsOccasion { get; set; } = null!;
    public IEnumerable<BookingRestaurantDto> BookingsRestaurant { get; set; } = null!;

    // Relation avec Login
    public virtual LoginDto Login { get; set; } = null!;

    // Relation with Address
    public virtual AddressDto? Address { get; set; }
}