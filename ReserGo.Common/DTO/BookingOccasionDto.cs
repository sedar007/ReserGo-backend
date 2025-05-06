namespace ReserGo.Common.DTO;
public class BookingOccasionDto {
    
    public int Id { get; set; }
    public int UserId { get; set; }
    public int OccasionId { get; set; }
    public DateTime BookingDate { get; set; }
    public String Status { get; set; } = null!;
    public Boolean VipAccess { get; set; } = false;
    
    public UserDto UserDto { get; set; } = null!;
    public OccasionDto OccasionDto { get; set; } = null!;
}