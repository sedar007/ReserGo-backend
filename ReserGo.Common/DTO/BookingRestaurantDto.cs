namespace ReserGo.Common.DTO;

public class BookingRestaurantDto {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RestaurantId { get; set; }

    public DateTime BookingDate { get; set; }
    public string Status { get; set; } = null!;
    public DateTime ReservationTime { get; set; }
    public int NumberOfPeople { get; set; }

    public UserDto UserDto { get; set; } = null!;
    public RestaurantDto RestaurantDto { get; set; } = null!;
}