using ReserGo.Common.Entity;

namespace ReserGo.Common.DTO;

public class RoomAvailabilityDto {
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Virtual
    public virtual RoomDto Room { get; set; } = null!;
    public virtual HotelDto Hotel { get; set; } = null!;
}