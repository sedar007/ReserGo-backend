using ReserGo.Common.Response;

namespace ReserGo.Business.Interfaces;

public interface IBookingService {
    Task<IEnumerable<BookingAllResponses>> GetBookingsByUserId(Guid userId);
}