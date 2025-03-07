namespace Common.DTO;
public class UserDTO {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public IEnumerable<BookingDTO> Bookings { get; set; }
}