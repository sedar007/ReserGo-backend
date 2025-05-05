using Microsoft.AspNetCore.Http;

namespace ReserGo.Common.Requests.Products.Occasion;

public class OccasionUpdateRequest {
    public string Name { get; init; }
    public string Location { get; init; }
    public int Capacity { get; init; }
    public IFormFile? Picture { get; init; }
}