namespace ReserGo.DataAccess.Interfaces;

using Common.Entity;

public interface IBookingRestaurantDataAccess {
    Task<BookingRestaurant> Create(BookingRestaurant bookingRestaurant);

    Task<BookingRestaurant?> GetById(Guid id);
}