using Microsoft.AspNetCore.Http;

namespace ReserGo.Common.Requests.Products;

public class HotelUpdateRequest {
    public string Name { get; init; }
    public string Location { get; init; }
    public int Capacity { get; init; }
    public IFormFile? Picture { get; init; }
}