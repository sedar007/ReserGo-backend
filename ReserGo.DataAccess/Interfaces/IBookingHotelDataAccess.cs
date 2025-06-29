using ReserGo.Common.Entity;
using ReserGo.Shared;
namespace ReserGo.DataAccess.Interfaces;

public interface IBookingHotelDataAccess {
    Task<BookingHotel> Create(BookingHotel bookingHotel);
    Task<IEnumerable<BookingHotel>> GetBookingsByRoomId(Guid roomId);
    Task<IEnumerable<BookingHotel>> GetBookingsByUserId(Guid userId, int pageSize = Consts.DefaultPageSize);
    Task<IEnumerable<BookingHotel>> GetBookingsByAdminId(Guid adminId);
    Task<IEnumerable<BookingHotel>> GetBookingYearsByUserId(Guid userId);
    Task<int> GetNbBookingBetween2DatesByAdminId(Guid adminId, DateOnly startDate, DateOnly endDate);
    Task<BookingHotel?> GetById(Guid id);
}