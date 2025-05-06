namespace ReserGo.Common.DTO;

public class RestaurantOfferDto {
    public int Id { get; set; }
    public string OfferTitle { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double? PricePerPerson { get; set; }
    public int NumberOfGuests { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public bool IsActive { get; set; }
    public int RestaurantId { get; set; }
    public RestaurantDto Restaurant { get; set; } = null!;
}