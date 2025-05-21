using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IRoomDataAccess {
    Task<Room?> GetById(Guid id);
    Task<Room> Create(Room room);
    Task<Room> Update(Room room);
    Task<IEnumerable<Room>> GetRoomsByHotelId(Guid hotelmId);
    Task Delete(Room room);
}