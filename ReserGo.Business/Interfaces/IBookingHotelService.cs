using Microsoft.AspNetCore.Http;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.User;
using ReserGo.Common.Enum;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Response;
using ReserGo.Common.Security;

namespace ReserGo.Business.Interfaces;

public interface IBookingHotelService {
    Task<BookingResponses> CreateBooking(BookingHotelRequest request, ConnectedUser user);
    /*Task<BookingHotelDto?> GetBookingById(Guid id);
    Task<IEnumerable<BookingHotelDto>> GetBookingsByUserId(Guid userId);
    Task<bool> ConfirmBooking(Guid bookingId, BookingStatus status);
    Task<bool> DeleteBooking(Guid bookingId); */
}