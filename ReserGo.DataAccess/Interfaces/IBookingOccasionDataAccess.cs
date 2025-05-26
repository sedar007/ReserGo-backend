namespace ReserGo.DataAccess.Interfaces;

using Common.Entity;

public interface IBookingOccasionDataAccess {
    Task<BookingOccasion> Create(BookingOccasion bookingOccasion);
    Task<IEnumerable<BookingOccasion>> GetBookingsByUserId(Guid userId);
    Task<IEnumerable<BookingOccasion>> GetBookingsByAdminId(Guid adminId);


    Task<int> GetNbBookingBetween2DatesByAdminId(Guid adminId, DateTime startDate, DateTime endDate);
    Task<int> GetNbBookingsLast30Days(Guid adminId);


    Task<BookingOccasion?> GetById(Guid id);
}