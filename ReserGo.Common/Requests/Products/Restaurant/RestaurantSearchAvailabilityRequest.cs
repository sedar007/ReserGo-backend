namespace ReserGo.Common.Requests.Products.Restaurant;

public class RestaurantSearchAvailabilityRequest {
    public int NumberOfGuests { get; init; } 
    public DateOnly Date { get; init; }
    public string? CuisineType { get; init; } = null!;
    
}
