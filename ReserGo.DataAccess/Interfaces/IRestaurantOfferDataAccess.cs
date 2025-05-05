using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IRestaurantOfferDataAccess {
    Task<RestaurantOffer?> GetById(int id);
    Task<RestaurantOffer> Create(RestaurantOffer restaurantOffer);
    Task<RestaurantOffer> Update(RestaurantOffer restaurantOffer);
    Task<IEnumerable<RestaurantOffer>> GetRestaurantsOfferByUserId(int userId);
    Task Delete(RestaurantOffer restaurantOffer);
}

