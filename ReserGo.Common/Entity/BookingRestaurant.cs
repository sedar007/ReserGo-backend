namespace ReserGo.Common.Entity;
public class BookingRestaurant {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RestaurantId { get; set; }
    
    public DateTime BookingDate { get; set; }
    public string Status { get; set; }
    public DateTime ReservationTime { get; set; }
    public int NumberOfPeople { get; set; }
    
    public User User { get; set; }
    public Restaurant Restaurant { get; set; }
}