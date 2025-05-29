namespace ReserGo.Common.DTO;

public class BookingEventDto : BookingDto {
    public Guid EventId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Virtual
    public virtual EventDto Event { get; set; } = null!;
}