using Microsoft.AspNetCore.Http;

namespace ReserGo.Common.Requests.Products.Restaurant;

public class RestaurantCreationRequest {
    public String Name { get; init; } = null!;
    public String Location { get; init; } = null!;
    public IFormFile? Picture { get; init; }
    public int Capacity { get; init; }
    public String CuisineType { get; init; } = null!;
    public long StayId { get; init; }
}