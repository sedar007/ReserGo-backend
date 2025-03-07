namespace Common.Entity;
public class BookingOccasion : Booking{
    public int OccasionId { get; set; }
    public bool VIPAccess { get; set; }
    public Occasion Occasion { get; set; }
}