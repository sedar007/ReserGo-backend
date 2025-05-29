namespace ReserGo.Common.Requests.Products.Event;

public class EventOfferCreationRequest {
    public string? Description { get; init; }
    public double PricePerPerson { get; init; }
    public int GuestLimit { get; init; }
    public DateTime OfferStartDate { get; init; }
    public DateTime OfferEndDate { get; init; }
    public bool IsActive { get; init; }
    public Guid EventId { get; init; }
}