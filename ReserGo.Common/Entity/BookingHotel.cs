namespace Common.Entity;
public class BookingHotel {
    public int HotelId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NumberOfGuests { get; set; }
    public Hotel Hotel { get; set; }
}