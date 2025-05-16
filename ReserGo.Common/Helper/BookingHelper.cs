using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class BookingHelper {
    public static BookingRestaurantDto ToDto(this BookingRestaurant bookingRestaurant) {
        return new BookingRestaurantDto {
            Id = bookingRestaurant.Id,
            UserId = bookingRestaurant.UserId,
            BookingDate = bookingRestaurant.BookingDate,
            NumberOfGuests = bookingRestaurant.NumberOfGuests,
            IsConfirmed = bookingRestaurant.IsConfirmed,
            CreatedAt = bookingRestaurant.CreatedAt,
            RestaurantOfferId = bookingRestaurant.RestaurantOfferId,
            RestaurantOffer = bookingRestaurant.RestaurantOffer?.ToDto()
        };
    }
    
    public static BookingHotelDto ToDto(this BookingHotel bookingHotel) {
        return new BookingHotelDto {
            Id = bookingHotel.Id,
            UserId = bookingHotel.UserId,
            BookingDate = bookingHotel.BookingDate,
            NumberOfGuests = bookingHotel.NumberOfGuests,
            IsConfirmed = bookingHotel.IsConfirmed,
            CreatedAt = bookingHotel.CreatedAt,
            HotelOfferId = bookingHotel.HotelOfferId,
            HotelOffer = bookingHotel.HotelOffer?.ToDto()
        };
    }
    
}