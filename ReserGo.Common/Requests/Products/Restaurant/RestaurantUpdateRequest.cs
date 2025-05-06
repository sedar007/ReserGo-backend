using Microsoft.AspNetCore.Http;

namespace ReserGo.Common.Requests.Products.Restaurant;

public class RestaurantUpdateRequest {
    public String Name { get; init; } = null!;
    public String Location { get; init; } = null!;
    public int Capacity { get; init; }
    public String CuisineType { get; init; } = null!;
    public IFormFile? Picture { get; init; }
}