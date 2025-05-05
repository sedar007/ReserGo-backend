using Microsoft.AspNetCore.Http;

namespace ReserGo.Common.Requests.Products;

public class HotelCreationRequest {
    public string Name { get; init; }
    public string Location { get; init; }
    public IFormFile? Picture { get; init; }
    public int Capacity { get; init; }
    public long StayId { get; init; }
}