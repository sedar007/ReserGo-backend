using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IHotelDataAccess
{
    Task<Hotel?> GetById(int id);
    Task<Hotel> Create(Hotel hotel);
}

