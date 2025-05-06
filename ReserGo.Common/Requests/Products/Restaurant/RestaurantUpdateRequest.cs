using Microsoft.AspNetCore.Http;

namespace ReserGo.Common.Requests.Products.Restaurant;

public class RestaurantUpdateRequest {
    public string Name { get; init; } = null!;
    public string Location { get; init; } = null!;
    public int Capacity { get; init; }
    public string CuisineType { get; init; } = null!;
    public IFormFile? Picture { get; init; }
}