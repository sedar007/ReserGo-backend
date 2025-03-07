namespace Common.Entity;
public class BookingRestaurant : Booking {
    public int RestaurantId { get; set; }
    public DateTime ReservationTime { get; set; }
    public int NumberOfPeople { get; set; }
    public Restaurant Restaurant { get; set; }
}