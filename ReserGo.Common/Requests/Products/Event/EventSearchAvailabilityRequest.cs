namespace ReserGo.Common.Requests.Products.Event;

public class EventSearchAvailabilityRequest {
    public int NumberOfGuests { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
}