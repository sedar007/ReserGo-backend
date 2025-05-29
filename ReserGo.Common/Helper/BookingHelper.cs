using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class BookingHelper {
    public static BookingRestaurantDto ToDto(this BookingRestaurant bookingRestaurant) {
        return new BookingRestaurantDto {
            Id = bookingRestaurant.Id,
            UserId = bookingRestaurant.UserId,
            Price = bookingRestaurant.Price,
            //  BookingDate = bookingRestaurant.BookingDate,
            NumberOfGuests = bookingRestaurant.NumberOfGuests,
            IsConfirmed = bookingRestaurant.IsConfirmed,
            BookingDate = bookingRestaurant.BookingDate,
            RestaurantOfferId = bookingRestaurant.RestaurantOfferId,
            RestaurantOffer = bookingRestaurant.RestaurantOffer?.ToDto(),
            Restaurant = bookingRestaurant?.Restaurant?.ToDto(),
            StartDate = bookingRestaurant.StartDate,
            EndDate = bookingRestaurant.EndDate
        };
    }

    public static BookingHotelDto ToDto(this BookingHotel bookingHotel) {
        return new BookingHotelDto {
            Id = bookingHotel.Id,
            UserId = bookingHotel.UserId,
            Price = bookingHotel.Price,
            NumberOfGuests = bookingHotel.NumberOfGuests,
            IsConfirmed = bookingHotel.IsConfirmed,
            BookingDate = bookingHotel.BookingDate,
            RoomId = bookingHotel.RoomId,
            HotelId = bookingHotel.HotelId,
            StartDate = bookingHotel.StartDate,
            EndDate = bookingHotel.EndDate,
            Hotel = bookingHotel?.Hotel.ToDto(),
            Room = bookingHotel?.Room.ToDto()
        };
    }

    public static BookingEventDto ToDto(this BookingEvent bookingEvent) {
        return new BookingEventDto {
            Id = bookingEvent.Id,
            UserId = bookingEvent.UserId,
            Price = bookingEvent.Price,
            NumberOfGuests = bookingEvent.NumberOfGuests,
            IsConfirmed = bookingEvent.IsConfirmed,
            BookingDate = bookingEvent.BookingDate,
            EventId = bookingEvent.EventId,
            StartDate = bookingEvent.StartDate,
            EndDate = bookingEvent.EndDate,
            Event = bookingEvent?.Event?.ToDto()
        };
    }
}