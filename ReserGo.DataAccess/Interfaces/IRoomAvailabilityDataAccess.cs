using ReserGo.Common.Entity;
using ReserGo.Common.Requests.Products.Hotel;

namespace ReserGo.DataAccess.Interfaces;

public interface IRoomAvailabilityDataAccess {
    Task<RoomAvailability> Create(RoomAvailability roomAvailability);
    Task<RoomAvailability> Update(RoomAvailability roomAvailability);
    Task<RoomAvailability?> GetByRoomId(Guid roomId);

    Task<IEnumerable<RoomAvailability>> GetAvailabilitiesByHotelId(Guid hotelId, int skip, int take);

    Task<IEnumerable<RoomAvailability>> GetAvailabilitiesByHotelIds(IEnumerable<Guid> hotelIds, int skip,
        int take);

    Task<IEnumerable<RoomAvailability>> GetAvailability(HotelSearchAvailabilityRequest request);

    Task<IEnumerable<RoomAvailability>> GetAvailabilitiesByRoomIdDate(Guid roomId, DateOnly startDate,
        DateOnly endDate);
}