namespace ReserGo.Common.DTO;

public class BookingRestaurantDto {
    
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RestaurantId { get; set; }
    
    public DateTime BookingDate { get; set; }
    public string Status { get; set; }
    public DateTime ReservationTime { get; set; }
    public int NumberOfPeople { get; set; }
    
    public UserDto UserDto { get; set; }
    public RestaurantDto RestaurantDto { get; set; }
}