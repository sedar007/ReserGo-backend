namespace ReserGo.Common.DTO;

public class RestaurantDto {
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string CuisineType { get; set; } = null!;
    public long StayId { get; set; }
    public int Capacity { get; set; }
    public string? Picture { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}