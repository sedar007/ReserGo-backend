namespace ReserGo.Common.Requests.Products.Event;

public class EventOfferCreationRequest {
    public string? Description { get; init; }
    public double PricePerPerson { get; init; }
    public int GuestLimit { get; init; }
    public DateOnly OfferStartDate { get; init; }
    public DateOnly OfferEndDate { get; init; }
    public bool IsActive { get; init; }
    public Guid EventId { get; init; }
}