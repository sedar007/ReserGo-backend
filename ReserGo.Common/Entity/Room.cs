namespace ReserGo.Common.Entity;

public class Room {
    public Guid Id { get; init; }
    public string RoomNumber { get; set; } = null!;
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public bool IsAvailable { get; set; } = true;
    public Guid HotelId { get; init; }
    public Hotel Hotel { get; init; } = null!;
}