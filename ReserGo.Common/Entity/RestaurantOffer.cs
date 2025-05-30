namespace ReserGo.Common.Entity;

public class RestaurantOffer {
    public Guid Id { get; init; }
    public string? Description { get; set; } = null!;
    public double? PricePerPerson { get; set; }
    public int GuestLimit { get; set; }
    public DateOnly OfferStartDate { get; set; }
    public DateOnly OfferEndDate { get; set; }
    public bool IsActive { get; set; }
    public Guid RestaurantId { get; init; }
    public Restaurant Restaurant { get; init; } = null!;
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;

    public ICollection<BookingRestaurant> Bookings { get; set; } = new List<BookingRestaurant>();
}