namespace ReserGo.DataAccess.Interfaces;
using ReserGo.Common.Entity;

public interface IBookingHotelDataAccess {
    Task<BookingHotel> Create(BookingHotel bookingHotel);
    Task<BookingHotel?> GetById(Guid id);
  /*  Task<BookingHotel?> GetById(Guid id);
    Task<BookingHotel?> GetByStayId(long stayId);
    
    Task<BookingHotel> Update(BookingHotel bookingHotel);
    Task<IEnumerable<BookingHotel>> GetBookingsByUserId(Guid userId);
    Task Delete(BookingHotel bookingHotel);*/
    
}