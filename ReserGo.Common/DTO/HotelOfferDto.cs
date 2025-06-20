namespace ReserGo.Common.DTO;

public class HotelOfferDto {
    public Guid Id { get; init; }
    public string OfferTitle { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double PricePerNight { get; set; }
    public int NumberOfGuests { get; set; }
    public int NumberOfRooms { get; set; }
    public DateOnly OfferStartDate { get; set; }
    public DateOnly OfferEndDate { get; set; }
    public bool IsActive { get; set; }

    public Guid HotelId { get; set; }
    public HotelDto Hotel { get; set; } = null!;

    public Guid UserId { get; set; }
}