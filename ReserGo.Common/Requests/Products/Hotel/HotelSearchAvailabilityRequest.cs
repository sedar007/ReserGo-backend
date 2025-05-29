namespace ReserGo.Common.Requests.Products.Hotel;

public class HotelSearchAvailabilityRequest {
    public int NumberOfPeople { get; init; } 
    public int NumberOfRooms { get; init; }
    public DateTime ArrivalDate { get; init; }
    public DateTime ReturnDate { get; init; }
}