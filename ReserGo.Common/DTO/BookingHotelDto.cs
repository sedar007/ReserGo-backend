namespace ReserGo.Common.DTO;

public class BookingHotelDto {
    public Guid Id { get; init; }
    public Guid HotelOfferId { get; set; }
    public HotelOfferDto HotelOffer { get; set; } = null!;
    public Guid UserId { get; set; }
    public UserDto User { get; set; } = null!;
    public DateTime BookingDate { get; set; }
    public int NumberOfGuests { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
}