namespace ReserGo.Common.DTO;

public class HotelDto {
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public long StayId { get; set; }
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int NumberOfRooms { get; set; }
    public string? Description { get; set; }
    public string? Picture { get; set; }
    public IEnumerable<RoomDto> Rooms { get; set; } = null!;
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}