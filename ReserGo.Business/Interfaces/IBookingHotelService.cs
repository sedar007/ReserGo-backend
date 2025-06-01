using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Response;
using ReserGo.Common.Security;
using ReserGo.Common.DTO;

namespace ReserGo.Business.Interfaces;

public interface IBookingHotelService {
    Task<BookingHotelResponses> CreateBooking(BookingHotelRequest request, ConnectedUser user);

    Task<IEnumerable<BookingHotelDto>> GetBookingsByUserId(Guid userId);
    Task<IEnumerable<BookingHotelDto>> GetBookingsByAdminId(Guid adminId);
}