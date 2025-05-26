namespace ReserGo.Common.DTO;

public class BookingOccasionDto : BookingDto {
    public Guid OccasionId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Virtual
    public virtual OccasionDto Occasion { get; set; } = null!;
}