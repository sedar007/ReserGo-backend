using ReserGo.Common.Entity;
using ReserGo.Common.Requests.Products.Restaurant;

namespace ReserGo.DataAccess.Interfaces;

public interface IRestaurantOfferDataAccess {
    Task<RestaurantOffer?> GetById(Guid id);
    Task<RestaurantOffer> Create(RestaurantOffer restaurantOffer);
    Task<RestaurantOffer> Update(RestaurantOffer restaurantOffer);
    Task<IEnumerable<RestaurantOffer>> GetRestaurantsOfferByUserId(Guid userId);

    Task<IEnumerable<RestaurantOffer>> SearchAvailability(RestaurantSearchAvailabilityRequest request);
    Task Delete(RestaurantOffer restaurantOffer);
}