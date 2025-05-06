using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products;
using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared.Interfaces;
using ReserGo.Shared;

namespace ReserGo.Business.Implementations;
public class RestaurantOfferService : IRestaurantOfferService {
    
    private readonly ILogger<RestaurantOfferService> _logger;
    private readonly ISecurity _security;
    private readonly IRestaurantService _restaurantService;
    private readonly IRestaurantOfferDataAccess _restaurantOfferDataAccess;
    private readonly IMemoryCache _cache;

    public RestaurantOfferService(ILogger<RestaurantOfferService> logger, 
                                  IRestaurantOfferDataAccess restaurantOfferDataAccess, 
                                  IRestaurantService restaurantService, 
                                  ISecurity security, 
                                  IMemoryCache cache) {
        _logger = logger;
        _security = security;
        _restaurantOfferDataAccess = restaurantOfferDataAccess;
        _restaurantService = restaurantService;
        _cache = cache;
    }
    
    public async Task<RestaurantOfferDto> Create(RestaurantOfferCreationRequest request) {
        try {
            
            string error = RestaurantOfferValidator.GetError(request);
            if (!string.IsNullOrEmpty(error)) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            ConnectedUser? connectedUser = _security.GetCurrentUser();
            if(connectedUser == null) throw new UnauthorizedAccessException("User not connected");
            
            RestaurantDto? restaurant = await _restaurantService.GetById(request.RestaurantId);
            if (restaurant == null) {
                string errorMessage = "Restaurant not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }
            
            RestaurantOffer newRestaurantOffer = new RestaurantOffer {
                OfferTitle = request.OfferTitle,
                Description = request.Description,
                PricePerPerson = request.PricePerPerson,
                NumberOfGuests = request.NumberOfGuests,
                OfferStartDate = request.OfferStartDate,
                OfferEndDate = request.OfferEndDate,
                IsActive = request.IsActive,
                RestaurantId = restaurant.Id,
                UserId = connectedUser.UserId
            };
            
            newRestaurantOffer = await _restaurantOfferDataAccess.Create(newRestaurantOffer);

            // Cache the created restaurant offer
            _cache.Set($"newRestaurantOffer_{newRestaurantOffer.Id}", newRestaurantOffer, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            
            _logger.LogInformation("Restaurant Offer { id } created", newRestaurantOffer.Id);
            return newRestaurantOffer.ToDto();
            
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
    public async Task<RestaurantOfferDto?> GetById(int id) {
        try {
            if (_cache.TryGetValue($"restaurantOffer_{id}", out RestaurantOffer cachedRestaurantOffer)) {
                return cachedRestaurantOffer.ToDto();
            }

            RestaurantOffer? restaurantOffer = await _restaurantOfferDataAccess.GetById(id);
            if (restaurantOffer is null) {
                string errorMessage = "This restaurant offer does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            _cache.Set($"restaurantOffer_{id}", restaurantOffer, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            
            _logger.LogInformation("Restaurant Offer { id } retrieved successfully", restaurantOffer.Id);
            return restaurantOffer.ToDto();
            
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
    
    public async Task<IEnumerable<RestaurantOfferDto>> GetRestaurantsByUserId(int userId) {
        try {
            string cacheKey = $"restaurantOffers_user_{userId}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<RestaurantOfferDto> cachedRestaurantOffers)) {
                return cachedRestaurantOffers;
            }

            IEnumerable<RestaurantOffer> restaurantOffers = await _restaurantOfferDataAccess.GetRestaurantsOfferByUserId(userId);
            IEnumerable<RestaurantOfferDto> restaurantOfferDtos = restaurantOffers.Select(restaurantOffer => restaurantOffer.ToDto());

            _cache.Set(cacheKey, restaurantOfferDtos, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            return restaurantOfferDtos;
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
   
    public async Task<RestaurantOfferDto> Update(int id, RestaurantOfferUpdateRequest request) {
        try {
            RestaurantOffer? restaurantOffer = await _restaurantOfferDataAccess.GetById(id);
            if (restaurantOffer is null) throw new Exception("Restaurant offer not found");

            string error = RestaurantOfferValidator.GetError(request);
            if (!string.IsNullOrEmpty(error)) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            restaurantOffer.OfferTitle = request.OfferTitle;
            restaurantOffer.Description = request.Description;
            restaurantOffer.PricePerPerson = request.PricePerPerson;
            restaurantOffer.NumberOfGuests = request.NumberOfGuests;
            restaurantOffer.OfferStartDate = request.OfferStartDate;
            restaurantOffer.OfferEndDate = request.OfferEndDate;
            restaurantOffer.IsActive = request.IsActive;

            restaurantOffer = await _restaurantOfferDataAccess.Update(restaurantOffer);

            // Update cache
            _cache.Set($"restaurant_offer_{restaurantOffer.Id}", restaurantOffer, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            
            _logger.LogInformation("Restaurant Offer { stayId } updated successfully", restaurantOffer.Id);
            return restaurantOffer.ToDto();
            
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task Delete(int id) {
        try {
            RestaurantOffer? restaurantOffer = await _restaurantOfferDataAccess.GetById(id);
            if (restaurantOffer is null) {
                string errorMessage = "Restaurant offer not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            await _restaurantOfferDataAccess.Delete(restaurantOffer);

            // Remove from cache
            _cache.Remove($"restaurant_offer_{restaurantOffer.Id}");
            _cache.Remove($"restaurantOffers_user_{restaurantOffer.UserId}");
            
            _logger.LogInformation("Restaurant Offer { id } deleted successfully", restaurantOffer.Id);
            
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
    
}
