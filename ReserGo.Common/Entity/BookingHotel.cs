namespace ReserGo.Common.Entity;
public class BookingHotel {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int HotelId { get; set; }
    public DateTime BookingDate { get; set; }
    public String Status { get; set; } = null!;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NumberOfGuests { get; set; }
    public Hotel Hotel { get; set; } = null!;
    public User User { get; set; } = null!;
    
    public DateTime? LastUpdated { get; set; } = DateTime.Now;
}