namespace ReserGo.Common.Entity;
public class User {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public IEnumerable<BookingHotel> BookingsHotel { get; set; }
    public IEnumerable<BookingOccasion> BookingsOccasion { get; set; }
    public IEnumerable<BookingRestaurant> BookingsRestaurant { get; set; }
}