namespace ReserGo.Common.DTO;

public class BookingOccasionDto {
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid OccasionId { get; set; }
    public DateTime BookingDate { get; set; }
    public string Status { get; set; } = null!;
    public bool VipAccess { get; set; } = false;

    public UserDto UserDto { get; set; } = null!;
    public OccasionDto OccasionDto { get; set; } = null!;
}