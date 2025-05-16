namespace ReserGo.Common.Entity;

public class BookingHotel : Booking {
    public Guid HotelOfferId { get; set; }
    public HotelOffer HotelOffer { get; set; } = null!;
}