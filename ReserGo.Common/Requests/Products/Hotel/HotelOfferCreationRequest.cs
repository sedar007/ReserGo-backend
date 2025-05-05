namespace ReserGo.Common.Requests.Products.Hotel;

public class HotelOfferCreationRequest {
   
    public string OfferTitle { get; set; }
    public string? Description { get; set; }
    public double PricePerNight { get; set; }
    public int NumberOfGuests { get; set; }
    public int NumberOfRooms { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public bool IsActive { get; set; }
    public int HotelId { get; set; }
   
}