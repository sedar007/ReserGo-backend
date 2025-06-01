using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.Common.Response;
namespace ReserGo.Business.Interfaces;

public interface IRestaurantOfferService {
    Task<RestaurantOfferDto?> GetById(Guid id);
    Task<RestaurantOfferDto> Create(RestaurantOfferCreationRequest request);
    Task<RestaurantOfferDto> Update(Guid id, RestaurantOfferUpdateRequest request);
    Task<RestaurantOfferDto> Update(RestaurantOfferDto restaurantOfferDto);
    Task<IEnumerable<RestaurantOfferDto>> GetRestaurantsByUserId(Guid userId);
    Task<IEnumerable<RestaurantAvailabilityResponse>> SearchAvailability(RestaurantSearchAvailabilityRequest request);
    Task Delete(Guid id);
}