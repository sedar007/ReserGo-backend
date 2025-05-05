using Microsoft.AspNetCore.Http;

namespace ReserGo.Common.Requests.Products.Occasion;

public class OccasionCreationRequest {
    public String Name { get; init; } = null!;
    public String Location { get; init; } = null!;
    public IFormFile? Picture { get; init; }
    public int Capacity { get; init; }
    public long StayId { get; init; }
}