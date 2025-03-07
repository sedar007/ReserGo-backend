namespace ReserGo.Common.DTO;
public class BookingOccasionDto {
    
    public int Id { get; set; }
    public int UserId { get; set; }
    public int OccasionId { get; set; }

    public DateTime BookingDate { get; set; }
    public string Status { get; set; }
    public bool VIPAccess { get; set; }
    
    public UserDto UserDto { get; set; }
    public OccasionDto OccasionDto { get; set; }
}