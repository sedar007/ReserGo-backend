using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Hotel.Rooms;
using ReserGo.Common.Security;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Response;

namespace ReserGo.Business.Interfaces;

public interface IRoomAvailabilityService {
    Task<RoomAvailabilityDto> SetAvailability(ConnectedUser connectedUser, Guid roomId,
        RoomAvailabilityRequest request);

    Task<IEnumerable<RoomAvailabilityDto>> GetAvailabilitiesByHotelId(ConnectedUser connectedUser,
        Guid hotelId, int skip, int take);

    Task<IEnumerable<RoomAvailabilityDto>> GetAvailabilitiesForAllHotels(ConnectedUser connectedUser, int skip,
        int take);


    Task<RoomAvailabilityDto> GetAvailabilityByRoomId(Guid roomId, DateOnly startDate, DateOnly endDate);
    Task<IEnumerable<RoomAvailibilityHotelResponse>> SearchAvailability(HotelSearchAvailabilityRequest request);


}