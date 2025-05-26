namespace ReserGo.DataAccess.Interfaces;

using Common.Entity;

public interface IBookingRestaurantDataAccess {
    Task<BookingRestaurant> Create(BookingRestaurant bookingRestaurant);

    Task<BookingRestaurant?> GetById(Guid id);
    Task<int> GetNbBookingBetween2DatesByAdminId(Guid adminId, DateTime startDate, DateTime endDate);
    Task<int> GetNbBookingsLast30Days(Guid adminId);
    Task<IEnumerable<BookingRestaurant>> GetBookingsByUserId(Guid userId);
    Task<IEnumerable<BookingRestaurant>> GetBookingsByAdminId(Guid adminId);
}