namespace ReserGo.Common.DTO;
public class UserDto {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public IEnumerable<BookingHotelDto> BookingsHotel { get; set; }
    public IEnumerable<BookingOccasionDto> BookingsOccasion { get; set; }
    public IEnumerable<BookingRestaurantDto> BookingsRestaurant { get; set; }
}