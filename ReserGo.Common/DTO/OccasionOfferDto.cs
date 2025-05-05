namespace ReserGo.Common.DTO;

public class OccasionOfferDto {
    public int Id { get; set; }
    public String OfferTitle { get; set; } = null!;
    public String Description { get; set; } = null!;
    public double Price { get; set; }
    public int NumberOfGuests { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public Boolean IsActive { get; set; }
    public int OccasionId { get; set; }
    public OccasionDto Occasion { get; set; } = null!;
   
}