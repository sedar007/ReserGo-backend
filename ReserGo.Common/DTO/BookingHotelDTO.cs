namespace Common.DTO;
public class BookingHotelDTO : BookingDTO {
    public int HotelId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NumberOfGuests { get; set; }
    public HotelDTO HotelDTO { get; set; }
}