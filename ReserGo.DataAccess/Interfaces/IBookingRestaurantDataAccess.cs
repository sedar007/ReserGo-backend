namespace ReserGo.DataAccess.Interfaces;

using Common.Entity;

public interface IBookingRestaurantDataAccess {
    Task<BookingRestaurant> Create(BookingRestaurant bookingRestaurant);

    Task<BookingRestaurant?> GetById(Guid id);
    Task<int> GetNbBookingBetween2DatesByAdminId(Guid adminId, DateOnly startDate, DateOnly endDate);
    Task<IEnumerable<BookingRestaurant>> GetBookingsByUserId(Guid userId);
    Task<IEnumerable<BookingRestaurant>> GetBookingYearsByUserId(Guid userId);
    Task<IEnumerable<BookingRestaurant>> GetBookingsByAdminId(Guid adminId);
}