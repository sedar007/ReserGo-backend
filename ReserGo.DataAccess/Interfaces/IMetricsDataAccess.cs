using ReserGo.Common.Entity;
using ReserGo.Common.Enum;
using ReserGo.Common.Response;

namespace ReserGo.DataAccess.Interfaces;

public interface IMetricsDataAccess {
    Task<Room?> GetById(Guid id);
    Task<Room> Create(Room room);
    Task<Room> Update(Room room);
    Task<IEnumerable<Room>> GetRoomsByHotelId(Guid hotelmId);
    Task Delete(Room room);
    Task<MetricsResponse> GetMetricsMonths(Product product, Guid userId);
}