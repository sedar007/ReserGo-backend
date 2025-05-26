using Microsoft.AspNetCore.Http;

namespace ReserGo.Common.Requests.Products.Hotel;

public class HotelCreationRequest {
    public string Name { get; init; } = null!;
    public string Location { get; init; } = null!;
    public IFormFile? Picture { get; init; }
    public string? Description { get; init; }
    public int NbRoomsVip { get; init; }
    public int NbRoomsStandard { get; init; }
    public int PriceVip { get; init; }
    public int PriceStandard { get; init; }
    public long StayId { get; init; }
}