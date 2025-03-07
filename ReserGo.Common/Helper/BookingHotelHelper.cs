using Common.DTO;
using Common.Entity;

namespace Common.Helper;

public static class BookingHotelHelper : BookingHelper {
    public static BookingHotelDTO ToDto(this BookingHotel bookingHotel) {
        return new BookingHotelDTO {
            HotelId = bookingHotel.HotelId,
            CheckIn = bookingHotel.CheckIn,
            CheckOut = bookingHotel.CheckOut,
            NumberOfGuests = bookingHotel.NumberOfGuests,
            Hotel = bookingHotel.Hotel
        };
    }
}