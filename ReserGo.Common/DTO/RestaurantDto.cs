namespace ReserGo.Common.DTO;
public class RestaurantDto {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string CuisineType { get; set; }
    public long StayId { get; set; }
    public int Capacity { get; set; } 
    public string? Picture { get; set; }
    public DateTime? LastUpdated { get; set; }
}