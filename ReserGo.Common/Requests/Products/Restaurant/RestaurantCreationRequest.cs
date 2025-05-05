using Microsoft.AspNetCore.Http;

namespace ReserGo.Common.Requests.Products.Restaurant;

public class RestaurantCreationRequest {
    public string Name { get; init; }
    public string Location { get; init; }
    public IFormFile? Picture { get; init; }
    public int Capacity { get; init; }
    public string CuisineType { get; init; }
    public long StayId { get; init; }
}