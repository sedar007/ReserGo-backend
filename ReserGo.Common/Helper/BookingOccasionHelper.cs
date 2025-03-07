using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;
public static class BookingOccasionHelper {
    public static BookingOccasionDto ToDto(this BookingOccasion bookingOccasion) {
        return new BookingOccasionDto {
            Id = bookingOccasion.Id,
            UserId = bookingOccasion.UserId,
            OccasionId = bookingOccasion.OccasionId,
            BookingDate = bookingOccasion.BookingDate,
            Status = bookingOccasion.Status,
            VIPAccess = bookingOccasion.VIPAccess
        };
    }
}