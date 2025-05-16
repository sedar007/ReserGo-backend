using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class BookingRestaurantHelper {
    public static BookingRestaurantDto ToDto(this BookingRestaurant bookingRestaurant) {
        return new BookingRestaurantDto {
            Id = bookingRestaurant.Id,
            RestaurantOfferId = bookingRestaurant.RestaurantOfferId,
            RestaurantOffer = bookingRestaurant.RestaurantOffer?.ToDto(),
            UserId = bookingRestaurant.UserId,
          //  User = bookingRestaurant.User.ToDto(),
            BookingDate = bookingRestaurant.BookingDate,
            NumberOfGuests = bookingRestaurant.NumberOfGuests,
            IsConfirmed = bookingRestaurant.IsConfirmed,
            CreatedAt = bookingRestaurant.CreatedAt
        };
    }
}