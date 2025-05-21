namespace ReserGo.DataAccess.Interfaces;

using Common.Entity;

public interface IBookingHotelDataAccess {
    Task<BookingHotel> Create(BookingHotel bookingHotel);
    Task<IEnumerable<BookingHotel>> GetBookingsByRoomId(Guid roomId);
    Task<IEnumerable<BookingHotel>> GetBookingsByUserId(Guid userId);
    Task<IEnumerable<BookingHotel>> GetBookingsByAdminId(Guid adminId);



    Task<BookingHotel?> GetById(Guid id);
}