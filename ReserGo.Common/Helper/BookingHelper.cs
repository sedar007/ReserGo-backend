using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class BookingHelper {
    public static BookingRestaurantDto ToDto(this BookingRestaurant bookingRestaurant) {
        return new BookingRestaurantDto {
            Id = bookingRestaurant.Id,
            UserId = bookingRestaurant.UserId,
            PriceTotal = bookingRestaurant.PriceTotal,
            PricePerPerson = bookingRestaurant.PricePerPerson,
            NumberOfGuests = bookingRestaurant.NumberOfGuests,
            IsConfirmed = bookingRestaurant.IsConfirmed,
            BookingDate = bookingRestaurant.BookingDate,
            RestaurantOfferId = bookingRestaurant.RestaurantOfferId,
            RestaurantOffer = bookingRestaurant.RestaurantOffer.ToDto(),
            Restaurant = bookingRestaurant.Restaurant.ToDto(),
            Date = bookingRestaurant.Date,
            User = bookingRestaurant.User?.ToDto()
        };
    }

    public static BookingHotelDto ToDto(this BookingHotel bookingHotel) {
        return new BookingHotelDto {
            Id = bookingHotel.Id,
            UserId = bookingHotel.UserId,
            PricePerPerson = bookingHotel.PricePerPerson,
            PriceTotal = bookingHotel.PriceTotal,
            NumberOfGuests = bookingHotel.NumberOfGuests,
            IsConfirmed = bookingHotel.IsConfirmed,
            BookingDate = bookingHotel.BookingDate,
            RoomId = bookingHotel.RoomId,
            HotelId = bookingHotel.HotelId,
            StartDate = bookingHotel.StartDate,
            EndDate = bookingHotel.EndDate,
            Hotel = bookingHotel?.Hotel.ToDto(),
            Room = bookingHotel?.Room.ToDto(),
            User = bookingHotel.User?.ToDto()
        };
    }

    public static BookingEventDto ToDto(this BookingEvent bookingEvent) {
        return new BookingEventDto {
            Id = bookingEvent.Id,
            UserId = bookingEvent.UserId,
            PriceTotal = bookingEvent.PriceTotal,
            PricePerDay = bookingEvent.PricePerDay,
            NumberOfGuests = bookingEvent.NumberOfGuests,
            IsConfirmed = bookingEvent.IsConfirmed,
            BookingDate = bookingEvent.BookingDate,
            EventOfferId = bookingEvent.EventOfferId,
            EventOffer = bookingEvent.EventOffer?.ToDto(),
            Event = bookingEvent?.Event.ToDto(),
            EventId = bookingEvent.EventId,
            StartDate = bookingEvent.StartDate,
            EndDate = bookingEvent.EndDate,
            User = bookingEvent.User?.ToDto()
        };
    }
}