using Microsoft.AspNetCore.Http;

namespace ReserGo.Common.Requests.Products.Occasion;

public class OccasionUpdateRequest {
    public String Name { get; init; } = null!;
    public String Location { get; init; } = null!;
    public int Capacity { get; init; }
    public IFormFile? Picture { get; init; }
}