namespace ReserGo.Common.Entity;
public class Restaurant {
    public int Id { get; set; }
    public long StayId { get; set; }
    public string Name { get; set; }
    public string CuisineType { get; set; }
    public int Capacity { get; set; } 
    public IEnumerable<BookingRestaurant> BookingRestaurant { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}