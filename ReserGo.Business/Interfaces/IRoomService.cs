using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products;
using ReserGo.Common.Requests.Products.Hotel.Rooms;

namespace ReserGo.Business.Interfaces;

public interface IRoomService {
    Task<RoomDto?> GetById(Guid id);
    Task<RoomDto> Create(RoomCreationRequest request);
    Task<RoomDto> Update(Guid id, RoomUpdateRequest request);
    Task<IEnumerable<RoomDto>> GetRoomsByHotelId(Guid hotelId);
    Task Delete(Guid id);
}