using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class BookingHotelHelper {
    public static BookingHotelDto ToDto(BookingHotel bookingHotel) {
        return new BookingHotelDto {
            Id = bookingHotel.Id,
            UserId = bookingHotel.UserId,
            HotelId = bookingHotel.HotelId,
            BookingDate = bookingHotel.BookingDate,
            Status = bookingHotel.Status,
            CheckIn = bookingHotel.CheckIn,
            CheckOut = bookingHotel.CheckOut,
            NumberOfGuests = bookingHotel.NumberOfGuests
        };
    }
}