using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.Common.Response;
using ReserGo.Common.Security;

namespace ReserGo.Business.Interfaces;

public interface IBookingRestaurantService {
    Task<BookingResponses> CreateBooking(BookingRestaurantRequest request, ConnectedUser user);
    Task<IEnumerable<BookingRestaurantDto>> GetBookingsByUserId(Guid userId);
    Task<IEnumerable<BookingRestaurantDto>> GetBookingsByAdminId(Guid adminId);
}