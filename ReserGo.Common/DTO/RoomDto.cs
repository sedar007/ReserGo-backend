namespace ReserGo.Common.DTO;

public class RoomDto {
    public Guid Id { get; init; }
    public string RoomNumber { get; set; } = null!;
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public bool IsAvailable { get; set; }
}