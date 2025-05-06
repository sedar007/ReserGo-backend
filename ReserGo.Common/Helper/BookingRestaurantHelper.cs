using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class BookingRestaurantHelper {
    public static BookingRestaurantDto ToDto(this BookingRestaurant bookingRestaurant) {
        return new BookingRestaurantDto {
            Id = bookingRestaurant.Id,
            UserId = bookingRestaurant.UserId,
            RestaurantId = bookingRestaurant.RestaurantId,
            BookingDate = bookingRestaurant.BookingDate,
            Status = bookingRestaurant.Status,
            ReservationTime = bookingRestaurant.ReservationTime,
            NumberOfPeople = bookingRestaurant.NumberOfPeople
        };
    }
}