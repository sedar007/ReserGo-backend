using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IHotelDataAccess {
    Task<Hotel?> GetById(Guid id);
    Task<Hotel?> GetByStayId(long stayId);
    Task<Hotel> Create(Hotel hotel);
    Task<Hotel> Update(Hotel hotel);
    Task<IEnumerable<Hotel>> GetHotelsByUserId(Guid userId);
    Task Delete(Hotel hotel);
}