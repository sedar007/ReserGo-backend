using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Requests.Products.Hotel.Rooms;
using ReserGo.Common.Response;
using ReserGo.Common.Security;

namespace ReserGo.Business.Interfaces;

public interface IRoomAvailabilityService {
    Task<IEnumerable<RoomAvailabilityDto>> SetAvailability(ConnectedUser connectedUser,
        RoomAvailabilitiesRequest request);

    Task<IEnumerable<RoomAvailabilityDto>> GetAvailabilitiesByHotelId(ConnectedUser connectedUser,
        Guid hotelId, int skip, int take);

    Task<IEnumerable<RoomAvailabilityDto>> GetAvailabilitiesForAllHotels(ConnectedUser connectedUser, int skip,
        int take);


    Task<RoomAvailabilityDto> GetAvailabilityByRoomId(Guid roomId, DateOnly startDate, DateOnly endDate);
    Task<IEnumerable<RoomAvailibilityHotelResponse>> SearchAvailability(HotelSearchAvailabilityRequest request);
}