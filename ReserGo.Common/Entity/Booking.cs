namespace Common.Entity;
public class Booking {
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime BookingDate { get; set; }
    public string Status { get; set; }
    public User User { get; set; }
}