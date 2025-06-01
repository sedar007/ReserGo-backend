using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Response;
using ReserGo.Common.Security;

namespace ReserGo.Business.Interfaces;

public interface IBookingHotelService {
    Task<BookingHotelResponses> CreateBooking(BookingHotelRequest request, ConnectedUser user);

    Task<IEnumerable<BookingHotelDto>> GetBookingsByUserId(Guid userId);
    Task<IEnumerable<BookingHotelDto>> GetBookingsByAdminId(Guid adminId);
}