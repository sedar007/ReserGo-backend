using Common.DTO;
using Common.Entity;

namespace Common.Helper;

public static class BookingOccasionHelper : BookingHelper {
    public static BookingOccasionDTO ToDto(this BookingOccasion bookingOccasion) {
        return new BookingOccasionDTO {
            HotelId = bookingOccasion.HotelId,
            CheckIn = bookingOccasion.CheckIn,
            CheckOut = bookingOccasion.CheckOut,
            NumberOfGuests = bookingOccasion.NumberOfGuests,
            Hotel = bookingOccasion.Hotel
        };
    }
}