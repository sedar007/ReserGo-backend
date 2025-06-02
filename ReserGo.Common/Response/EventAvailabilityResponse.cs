namespace ReserGo.Common.Response;

public class EventAvailabilityResponse {
    public Guid EventOfferId { get; set; }
    public string EventName { get; set; } = null!;
    public double PricePerDay { get; set; }
    public int AvailableCapacity { get; set; }
    public string ImageSrc { get; set; } = string.Empty;
}