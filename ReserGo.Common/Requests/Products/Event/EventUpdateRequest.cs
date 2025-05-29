using Microsoft.AspNetCore.Http;

namespace ReserGo.Common.Requests.Products.Event;

public class EventUpdateRequest {
    public string Name { get; init; } = null!;
    public string Location { get; init; } = null!;
    public int Capacity { get; init; }
    public IFormFile? Picture { get; init; }
}