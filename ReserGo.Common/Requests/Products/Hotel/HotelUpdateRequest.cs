using Microsoft.AspNetCore.Http;

namespace ReserGo.Common.Requests.Products.Hotel;

public class HotelUpdateRequest {
    public string Name { get; init; } = null!;
    public string Location { get; init; } = null!;
    public IFormFile? Picture { get; init; }
}