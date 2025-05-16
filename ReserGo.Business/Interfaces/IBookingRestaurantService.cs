using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.Common.Response;
using ReserGo.Common.Security;

namespace ReserGo.Business.Interfaces;

public interface IBookingRestaurantService {
    Task<BookingResponses> CreateBooking(BookingRestaurantRequest request, ConnectedUser user);
}