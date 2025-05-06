namespace ReserGo.Common.DTO;

public class BookingOccasionDto {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int OccasionId { get; set; }
    public DateTime BookingDate { get; set; }
    public string Status { get; set; } = null!;
    public bool VipAccess { get; set; } = false;

    public UserDto UserDto { get; set; } = null!;
    public OccasionDto OccasionDto { get; set; } = null!;
}