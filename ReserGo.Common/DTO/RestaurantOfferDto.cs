namespace ReserGo.Common.DTO;

public class RestaurantOfferDto {
    public int Id { get; set; }
    public String OfferTitle { get; set; } = null!;
    public String Description { get; set; } = null!;
    public double? PricePerPerson { get; set; }
    public int NumberOfGuests { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public Boolean IsActive { get; set; }
    public int RestaurantId { get; set; }
    public RestaurantDto Restaurant { get; set; } = null!;
   
}