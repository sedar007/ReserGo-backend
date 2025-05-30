namespace ReserGo.Common.Requests.Products.Hotel;

public class HotelSearchAvailabilityRequest {
    public int NumberOfPeople { get; init; } 
    public int NumberOfRooms { get; init; }
    public DateOnly ArrivalDate { get; init; }
    public DateOnly ReturnDate { get; init; }
}