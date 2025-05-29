using Microsoft.AspNetCore.Http;

namespace ReserGo.Common.Requests.Products.Event;

public class EventCreationRequest {
    public string Name { get; init; } = null!;
    public string Location { get; init; } = null!;
    public IFormFile? Picture { get; init; }
    public int Capacity { get; init; }
    public long StayId { get; init; }
}