using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IBookingEventDataAccess {
    Task<BookingEvent> Create(BookingEvent bookingEvent);
    Task<IEnumerable<BookingEvent>> GetBookingsByUserId(Guid userId, int pageSize = 10);
    Task<IEnumerable<BookingEvent>> GetBookingsByAdminId(Guid adminId);
    Task<IEnumerable<BookingEvent>> GetBookingYearsByUserId(Guid userId);


    Task<int> GetNbBookingBetween2DatesByAdminId(Guid adminId, DateOnly startDate, DateOnly endDate);

    Task<BookingEvent?> GetById(Guid id);
}