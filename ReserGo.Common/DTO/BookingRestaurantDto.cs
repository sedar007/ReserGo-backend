namespace ReserGo.Common.DTO;

public class BookingRestaurantDto {
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid RestaurantId { get; set; }

    public DateTime BookingDate { get; set; }
    public string Status { get; set; } = null!;
    public DateTime ReservationTime { get; set; }
    public int NumberOfPeople { get; set; }

    public UserDto UserDto { get; set; } = null!;
    public RestaurantDto RestaurantDto { get; set; } = null!;
}