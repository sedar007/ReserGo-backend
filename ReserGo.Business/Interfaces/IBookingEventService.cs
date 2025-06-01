using ReserGo.Common.Requests.Products.Event;
using ReserGo.Common.Response;
using ReserGo.Common.Security;
using ReserGo.Common.DTO;

namespace ReserGo.Business.Interfaces;

public interface IBookingEventService {
    Task<BookingResponses> CreateBooking(BookingEventRequest request, ConnectedUser user);
    //Task<IEnumerable<BookingEventDto>> GetBookingsByUserId(Guid userId);
    Task<IEnumerable<BookingEventDto>> GetBookingsByAdminId(Guid adminId);
}