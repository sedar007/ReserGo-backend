namespace ReserGo.DataAccess.Interfaces;

using Common.Entity;

public interface IBookingEventDataAccess {
    Task<BookingEvent> Create(BookingEvent bookingEvent);
    Task<IEnumerable<BookingEvent>> GetBookingsByUserId(Guid userId);
    Task<IEnumerable<BookingEvent>> GetBookingsByAdminId(Guid adminId);
    Task<IEnumerable<BookingEvent>> GetBookingYearsByUserId(Guid userId);


    Task<int> GetNbBookingBetween2DatesByAdminId(Guid adminId, DateTime startDate, DateTime endDate);
    Task<int> GetNbBookingsLast30Days(Guid adminId);


    Task<BookingEvent?> GetById(Guid id);
}