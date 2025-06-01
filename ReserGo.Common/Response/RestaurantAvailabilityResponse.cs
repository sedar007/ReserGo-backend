namespace ReserGo.Common.Response;

public class RestaurantAvailabilityResponse {
    
    public Guid RestaurantOfferId { get; set; }
    public string RestaurantName { get; set; } = null!;
    public string? TypeOfCuisine { get; set; } = null!;
    public double? PricePerGuest { get; set; }
    public int AvailableCapacity { get; set; }
    public string ImageSrc { get; set; } = null!;
    
}
