namespace ReserGo.Common.DTO;

public class BookingHotelDto : BookingDto{
    public Guid RoomId { get; set; }
    public Guid HotelId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    // Virtual
    public virtual RoomDto Room { get; set; } = null!;
    public virtual HotelDto Hotel { get; set; } = null!;
}