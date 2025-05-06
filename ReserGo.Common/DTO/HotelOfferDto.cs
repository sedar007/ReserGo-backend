namespace ReserGo.Common.DTO;

public class HotelOfferDto {
    public int Id { get; set; }
    public String OfferTitle { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double PricePerNight { get; set; }
    public int NumberOfGuests { get; set; }
    public int NumberOfRooms { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public bool IsActive { get; set; }
    
    public int HotelId { get; set; }
    public HotelDto Hotel { get; set; } = null!;
}