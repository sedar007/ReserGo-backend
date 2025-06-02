namespace ReserGo.Common.DTO;

public class BookingEventDto : BookingDto {
    public Guid EventId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public double PricePerDay { get; set; }
    public Guid EventOfferId { get; set; }
    public EventOfferDto? EventOffer { get; set; }

    public virtual EventDto Event { get; set; } = null!;
}