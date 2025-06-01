using ReserGo.Common.Entity;

namespace ReserGo.Common.DTO;

public class RoomAvailabilityDto {
    public Guid Id { get; set; }
    public string? Description { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    // Virtual
    public virtual RoomDto Room { get; set; } = null!;
    public virtual HotelDto Hotel { get; set; } = null!;
}