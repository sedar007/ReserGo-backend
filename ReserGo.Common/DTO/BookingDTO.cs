namespace Common.DTO;
public class BookingDTO {
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime BookingDate { get; set; }
    public string Status { get; set; }
    public UserDTO UserDTO { get; set; }
}