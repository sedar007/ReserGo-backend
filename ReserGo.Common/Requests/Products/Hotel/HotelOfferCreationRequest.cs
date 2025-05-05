namespace ReserGo.Common.Requests.Products.Hotel;

public class HotelOfferCreationRequest {
   
    public String OfferTitle { get; init; } = null!;
    public String? Description { get; init; }
    public double PricePerNight { get; init; }
    public int NumberOfGuests { get; init; }
    public int NumberOfRooms { get; init; }
    public DateTime OfferStartDate { get; init; }
    public DateTime OfferEndDate { get; init; }
    public bool IsActive { get; init; }
    public int HotelId { get; init; }
   
}