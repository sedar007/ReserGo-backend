namespace ReserGo.Common.DTO;

public class HotelDto {
    public Guid Id { get; init; } 
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int Capacity { get; set; }
    public string? Picture { get; set; }
    public long StayId { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}