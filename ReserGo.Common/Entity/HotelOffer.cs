namespace ReserGo.Common.Entity;

public class HotelOffer {
    public int Id { get; set; }
    public string OfferTitle { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double PricePerNight { get; set; }
    public int NumberOfGuests { get; set; }
    public int NumberOfRooms { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public bool IsActive { get; set; }

    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}