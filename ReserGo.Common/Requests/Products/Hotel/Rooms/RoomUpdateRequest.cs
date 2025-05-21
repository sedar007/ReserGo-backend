namespace ReserGo.Common.Requests.Products.Hotel.Rooms;

public class RoomUpdateRequest {
    public string RoomNumber { get; init; } = null!;
    public int Capacity { get; init; }
    public decimal PricePerNight { get; init; }
    public bool IsAvailable { get; init; }
}