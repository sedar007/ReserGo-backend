namespace ReserGo.Common.DTO;
public class OccasionDto {
    public int Id { get; set; }
    public String Name { get; set; } = null!;
    public String Location { get; set; } = null!;
    public long StayId { get; set; }
    public int Capacity { get; set; } 
    public String? Picture { get; set; }
    public DateTime LastUpdated { get; set; }
}