namespace ReserGo.Common.Entity;
public class BookingHotel {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int HotelId { get; set; }
    
    public DateTime BookingDate { get; set; }
    public string Status { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NumberOfGuests { get; set; }
    public Hotel Hotel { get; set; }
    public User User { get; set; }
}