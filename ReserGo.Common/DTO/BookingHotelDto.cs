namespace ReserGo.Common.DTO;
public class BookingHotelDto {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int HotelId { get; set; }
    public DateTime BookingDate { get; set; }
    public string Status { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NumberOfGuests { get; set; }
    
    public HotelDto HotelDto { get; set; }
    public UserDto UserDto { get; set; }
}