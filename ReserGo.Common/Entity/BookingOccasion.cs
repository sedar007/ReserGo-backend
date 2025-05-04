namespace ReserGo.Common.Entity;
public class BookingOccasion {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int OccasionId { get; set; }

    public DateTime BookingDate { get; set; }
    public string Status { get; set; }
    public bool VIPAccess { get; set; }
    
    public User User { get; set; }
    public Occasion Occasion { get; set; }
    
    public DateTime? LastUpdated { get; set; }
}