namespace ReserGo.Common.DTO;

public class EventOfferDto {
    public Guid Id { get; init; }
    public string? Description { get; set; } = null!;
    public double PricePerPerson { get; set; }
    public int GuestLimit { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public bool IsActive { get; set; }
    public Guid EventId { get; set; }
    public EventDto Event { get; set; } = null!;
}