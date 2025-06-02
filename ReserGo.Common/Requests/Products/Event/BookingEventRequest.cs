namespace ReserGo.Common.Requests.Products.Event;

public class BookingEventRequest {
    public Guid EventOfferId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}