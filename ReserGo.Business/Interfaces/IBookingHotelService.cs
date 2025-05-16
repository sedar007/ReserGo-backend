using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Response;
using ReserGo.Common.Security;

namespace ReserGo.Business.Interfaces;

public interface IBookingHotelService {
    Task<BookingResponses> CreateBooking(BookingHotelRequest request, ConnectedUser user);
}