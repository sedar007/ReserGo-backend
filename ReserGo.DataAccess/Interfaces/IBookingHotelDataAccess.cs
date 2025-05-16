namespace ReserGo.DataAccess.Interfaces;

using Common.Entity;

public interface IBookingHotelDataAccess {
    Task<BookingHotel> Create(BookingHotel bookingHotel);

    Task<BookingHotel?> GetById(Guid id);
}