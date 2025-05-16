namespace ReserGo.Common.DTO;

public class BookingHotelDto : BookingDto{
    public Guid HotelOfferId { get; set; }
    public HotelOfferDto? HotelOffer { get; set; }
}