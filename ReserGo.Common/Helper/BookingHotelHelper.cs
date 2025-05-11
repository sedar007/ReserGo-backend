using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class BookingHotelHelper {
    public static BookingHotelDto ToDto(this BookingHotel bookingHotel) {
        return new BookingHotelDto {
            Id = bookingHotel.Id,
            HotelOfferId = bookingHotel.HotelOfferId,
            HotelOffer = bookingHotel.HotelOffer?.ToDto(),
            UserId = bookingHotel.UserId,
           // User = bookingHotel.User?.ToDto(),
            BookingDate = bookingHotel.BookingDate,
            NumberOfGuests = bookingHotel.NumberOfGuests,
            IsConfirmed = bookingHotel.IsConfirmed,
            CreatedAt = bookingHotel.CreatedAt
        };
    }
}