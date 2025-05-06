namespace ReserGo.Common.Entity;

public class RestaurantOffer {
    public int Id { get; set; }
    public String OfferTitle { get; set; } = null!;
    public String Description { get; set; } = null!;
    public double? PricePerPerson { get; set; }
    public int NumberOfGuests { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public bool IsActive { get; set; }
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
