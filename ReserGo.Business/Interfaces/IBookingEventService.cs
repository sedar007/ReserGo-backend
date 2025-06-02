using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Event;
using ReserGo.Common.Response;
using ReserGo.Common.Security;

namespace ReserGo.Business.Interfaces;

public interface IBookingEventService {
    Task<BookingResponses> CreateBooking(BookingEventRequest request, ConnectedUser user);

    Task<IEnumerable<BookingAllResponses>> GetBookingsByUserId(Guid userId);

    Task<IEnumerable<BookingEventDto>> GetBookingsByAdminId(Guid adminId);
}