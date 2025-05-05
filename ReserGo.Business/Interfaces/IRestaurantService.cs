using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Restaurant;

namespace ReserGo.Business.Interfaces;

public interface IRestaurantService {
    Task<RestaurantDto?> GetById(int id);
    Task<RestaurantDto?> GetByStayId(long stayId);
    Task<IEnumerable<RestaurantDto>> GetRestaurantsByUserId(int userId);
    Task<RestaurantDto> Create(RestaurantCreationRequest request);
    Task<RestaurantDto> Update(long stayId, RestaurantUpdateRequest request);
    Task Delete(int id);
}