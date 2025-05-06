namespace ReserGo.Common.DTO;
public class HotelDto {
    public int Id { get; set; }
    public String Name { get; set; } = null!;
    public String Location { get; set; } = null!;
    public int Capacity { get; set; }
    public string? Picture { get; set; }
    public long StayId { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}