using Common.DTO;
using Common.Entity;

namespace Common.Helper;
public static class BookingHelper {
    public static BookingDTO ToDto(this Booking booking) {
        return new BookingDTO {
            Id = booking.Id,
            UserId = booking.UserId,
            BookingDate = booking.BookingDate,
            Status = booking.Status,
            User = UserHelper::toDto(booking.User)
        };
    }
}