namespace ReserGo.Common.DTO;

public class HotelOfferDto {
    public Guid Id { get; init; } = Guid.NewGuid();
    public string OfferTitle { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double PricePerNight { get; set; }
    public int NumberOfGuests { get; set; }
    public int NumberOfRooms { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public bool IsActive { get; set; }

    public Guid HotelId { get; set; }
    public HotelDto Hotel { get; set; } = null!;
}