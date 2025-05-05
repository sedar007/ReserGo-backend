using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IRestaurantDataAccess {
    Task<Restaurant?> GetById(int id);
    Task<Restaurant?> GetByStayId(long stayId);
    
    Task<Restaurant> Create(Restaurant restaurant);
    Task<Restaurant> Update(Restaurant restaurant);
    Task Delete(Restaurant restaurant);
}