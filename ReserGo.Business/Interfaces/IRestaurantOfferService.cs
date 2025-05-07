using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Restaurant;

namespace ReserGo.Business.Interfaces;

public interface IRestaurantOfferService {
    Task<RestaurantOfferDto?> GetById(Guid id);
    Task<RestaurantOfferDto> Create(RestaurantOfferCreationRequest request);
    Task<RestaurantOfferDto> Update(Guid id, RestaurantOfferUpdateRequest request);
    Task<IEnumerable<RestaurantOfferDto>> GetRestaurantsByUserId(Guid userId);
    Task Delete(Guid id);
}