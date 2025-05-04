using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IHotelDataAccess {
    Task<Hotel?> GetById(int id);
    Task<Hotel?> GetByStayId(long stayId);
    Task<Hotel> Create(Hotel hotel);
    Task<Hotel> Update(Hotel hotel);
    Task Delete(Hotel hotel);
}

