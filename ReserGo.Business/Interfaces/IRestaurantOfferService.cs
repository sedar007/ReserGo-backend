using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Restaurant;

namespace ReserGo.Business.Interfaces;

public interface IRestaurantOfferService {
    Task<RestaurantOfferDto?> GetById(int id);
    Task<RestaurantOfferDto> Create(RestaurantOfferCreationRequest request);
    Task<RestaurantOfferDto> Update(int id, RestaurantOfferUpdateRequest request);
    Task<IEnumerable<RestaurantOfferDto>> GetRestaurantsByUserId(int userId);
    Task Delete(int id);
}