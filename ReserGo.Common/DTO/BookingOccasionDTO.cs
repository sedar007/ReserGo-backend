namespace Common.DTO;
public class BookingOccasionDTO : BookingDTO {
    public int OccasionId { get; set; }
    public bool VIPAccess { get; set; }
    public OccasionDTO OccasionDTO { get; set; }
}