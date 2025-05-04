namespace ReserGo.Common.DTO;
public class HotelDto {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public int Capacity { get; set; }
    public string? Picture { get; set; }
    public DateTime? LastUpdated { get; set; }
}