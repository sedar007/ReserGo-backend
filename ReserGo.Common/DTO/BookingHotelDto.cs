namespace ReserGo.Common.DTO;

public class BookingHotelDto : BookingDto {
    public Guid RoomId { get; set; }
    public Guid HotelId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    // Virtual
    public virtual RoomDto Room { get; set; } = null!;
    public virtual HotelDto Hotel { get; set; } = null!;
}