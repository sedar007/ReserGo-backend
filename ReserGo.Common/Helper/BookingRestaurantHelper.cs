using Common.DTO;
using Common.Entity;
using Common.Helper;

namespace Common.Helper;

public class BookingRestaurantHelper : BookingHelper {
    public static BookingRestaurantDTO ToDto(this BookingRestaurant bookingRestaurant) {
        return new BookingRestaurantDTO {
            RestaurantId = bookingRestaurant.RestaurantId,
            ReservationTime = bookingRestaurant.ReservationTime,
            NumberOfPeople = bookingRestaurant.NumberOfPeople,
            Restaurant = bookingRestaurant.Restaurant
        };
    }
}
