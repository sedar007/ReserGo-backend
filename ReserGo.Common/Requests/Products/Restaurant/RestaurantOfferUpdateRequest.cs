namespace ReserGo.Common.Requests.Products.Restaurant;

public class RestaurantOfferUpdateRequest {
    public string OfferTitle { get; set; }
    public string? Description { get; set; }
    public double? PricePerPerson { get; set; }
    public int NumberOfGuests { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public bool IsActive { get; set; }
   
}