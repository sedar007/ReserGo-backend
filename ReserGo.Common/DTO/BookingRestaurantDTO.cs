namespace Common.DTO;

public class BookingRestaurantDTO : BookingDTO {
    public int RestaurantId { get; set; }
    public DateTime ReservationTime { get; set; }
    public int NumberOfPeople { get; set; }
    public RestaurantDTO RestaurantDTO { get; set; }
}