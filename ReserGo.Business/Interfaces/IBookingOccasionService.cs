using ReserGo.Common.Requests.Products.Occasion;
using ReserGo.Common.Response;
using ReserGo.Common.Security;
using ReserGo.Common.DTO;

namespace ReserGo.Business.Interfaces;

public interface IBookingOccasionService {
    // Task<BookingResponses> CreateBooking(BookingOccasionRequest request, ConnectedUser user);

    Task<IEnumerable<BookingOccasionDto>> GetBookingsByUserId(Guid userId);
    Task<IEnumerable<BookingOccasionDto>> GetBookingsByAdminId(Guid adminId);
}