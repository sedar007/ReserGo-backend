using ReserGo.Common.Entity;
using ReserGo.Shared;
namespace ReserGo.DataAccess.Interfaces;

public interface IBookingRestaurantDataAccess {
    Task<BookingRestaurant> Create(BookingRestaurant bookingRestaurant);

    Task<BookingRestaurant?> GetById(Guid id);
    Task<int> GetNbBookingBetween2DatesByAdminId(Guid adminId, DateOnly startDate, DateOnly endDate);
    Task<IEnumerable<BookingRestaurant>> GetBookingsByUserId(Guid userId, int pageSize = Consts.DefaultPageSize);
    Task<IEnumerable<BookingRestaurant>> GetBookingYearsByUserId(Guid userId);
    Task<IEnumerable<BookingRestaurant>> GetBookingsByAdminId(Guid adminId);
}