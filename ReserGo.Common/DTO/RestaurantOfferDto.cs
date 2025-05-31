namespace ReserGo.Common.DTO;

public class RestaurantOfferDto {
    public Guid Id { get; init; }
    public string? Description { get; set; } = null!;
    public double PricePerPerson { get; set; }
    public int GuestNumber { get; set; } = 0;
    public int GuestLimit { get; set; }
    public DateOnly OfferStartDate { get; set; }
    public DateOnly OfferEndDate { get; set; }
    public bool IsActive { get; set; }
    public Guid RestaurantId { get; set; }
    public RestaurantDto Restaurant { get; set; } = null!;

    public Guid UserId { get; set; }
}