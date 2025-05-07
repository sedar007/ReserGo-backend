namespace ReserGo.Common.DTO;

public class OccasionDto {
    public Guid Id { get; init; } 
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public long StayId { get; set; }
    public int Capacity { get; set; }
    public string? Picture { get; set; }
    public DateTime LastUpdated { get; set; }
}