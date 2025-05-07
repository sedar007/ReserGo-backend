namespace ReserGo.Common.DTO;

public class BookingHotelDto {
    public Guid Id { get; init; }
    public Guid UserId { get; set; }
    public Guid HotelId { get; set; }
    public DateTime BookingDate { get; set; } = DateTime.Now;
    public string Status { get; set; } = null!;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NumberOfGuests { get; set; }
    public HotelDto HotelDto { get; set; } = null!;
    public UserDto UserDto { get; set; } = null!;
}