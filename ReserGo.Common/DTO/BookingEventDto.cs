namespace ReserGo.Common.DTO;

public class BookingEventDto : BookingDto {
    public Guid EventId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    // Virtual
    public virtual EventDto Event { get; set; } = null!;
}