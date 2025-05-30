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
            var error = RestaurantOfferValidator.GetError(request);
            if (!string.IsNullOrEmpty(error)) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) throw new UnauthorizedAccessException("User not connected");

            var restaurant = await _restaurantService.GetById(request.RestaurantId);
            if (restaurant == null) {
                var errorMessage = "Restaurant not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            if (request.GuestLimit < 1) {
                var errorMessage = "Number of guests must be greater than 0";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            if (request.PricePerPerson != null && request.PricePerPerson < 0) {
                var errorMessage = "Price per person must be greater than or equal to 0";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            if (request.OfferStartDate < DateOnly.FromDateTime(DateTime.UtcNow)) {
                var errorMessage = "Offer start date must be greater than or equal to today";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            if (request.OfferEndDate < request.OfferStartDate) {
                var errorMessage = "Offer end date must be greater than or equal to offer start date";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            if (request.OfferEndDate < DateOnly.FromDateTime(DateTime.UtcNow)) {
                var errorMessage = "Offer end date must be greater than or equal to today";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            if (request.OfferStartDate > request.OfferEndDate) {
                var errorMessage = "Offer start date must be less than or equal to offer end date";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            if (restaurant.Capacity < request.GuestLimit) {
                var errorMessage = "Number of guests must be greater than or equal to restaurant capacity";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var newRestaurantOffer = new RestaurantOffer {
                Description = request.Description,
                PricePerPerson = request.PricePerPerson,
                GuestLimit = request.GuestLimit,
                OfferStartDate = request.OfferStartDate,
                OfferEndDate = request.OfferEndDate,
                IsActive = request.IsActive,
                RestaurantId = restaurant.Id,
                UserId = connectedUser.UserId
            };

            newRestaurantOffer = await _restaurantOfferDataAccess.Create(newRestaurantOffer);
            var cacheKey = Consts.RestaurantOffersCacheKey.Concat(newRestaurantOffer.Id.ToString());

            RemoveCache(newRestaurantOffer.Id, connectedUser.UserId);
            // Cache the created restaurant offer
            _cache.Set(cacheKey, newRestaurantOffer,
                TimeSpan.FromMinutes(Consts.CacheDurationMinutes));


            _logger.LogInformation("Restaurant Offer { id } created", newRestaurantOffer.Id);
            return newRestaurantOffer.ToDto();
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<RestaurantOfferDto?> GetById(Guid id) {
        try {
            if (id == Guid.Empty) {
                var errorMessage = "Invalid restaurant offer id";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var cacheKey = Consts.RestaurantOffersCacheKey.Concat(id.ToString());
            if (_cache.TryGetValue(cacheKey, out RestaurantOffer cachedRestaurantOffer))
                return cachedRestaurantOffer.ToDto();

            var restaurantOffer = await _restaurantOfferDataAccess.GetById(id);
            if (restaurantOffer is null) {
                var errorMessage = "This restaurant offer does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            _cache.Set(cacheKey, restaurantOffer, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            _logger.LogInformation("Restaurant Offer { id } retrieved successfully", restaurantOffer.Id);
            return restaurantOffer.ToDto();
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<IEnumerable<RestaurantOfferDto>> GetRestaurantsByUserId(Guid userId) {
        try {
            var cacheKey = Consts.RestaurantOffersUserIdCacheKey.Concat(userId.ToString());

            if (_cache.TryGetValue(cacheKey, out IEnumerable<RestaurantOfferDto> cachedRestaurantOffers))
                return cachedRestaurantOffers;

            var restaurantOffers = await _restaurantOfferDataAccess.GetRestaurantsOfferByUserId(userId);
            IEnumerable<RestaurantOfferDto> restaurantOfferDtos =
                restaurantOffers.Select(restaurantOffer => restaurantOffer.ToDto());

            _cache.Set(cacheKey, restaurantOfferDtos, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            return restaurantOfferDtos;
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<RestaurantOfferDto> Update(Guid id, RestaurantOfferUpdateRequest request) {
        try {
            var restaurantOffer = await _restaurantOfferDataAccess.GetById(id);
            if (restaurantOffer is null) throw new Exception("Restaurant offer not found");

            var error = RestaurantOfferValidator.GetError(request);
            if (!string.IsNullOrEmpty(error)) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            restaurantOffer.Description = request.Description;
            restaurantOffer.PricePerPerson = request.PricePerPerson;
            restaurantOffer.GuestLimit = request.GuestLimit;
            restaurantOffer.OfferStartDate = request.OfferStartDate;
            restaurantOffer.OfferEndDate = request.OfferEndDate;
            restaurantOffer.IsActive = request.IsActive;

            restaurantOffer = await _restaurantOfferDataAccess.Update(restaurantOffer);

            // Update cache
            var cacheKey = Consts.RestaurantOffersCacheKey.Concat(restaurantOffer.Id.ToString());
            _cache.Set(cacheKey, restaurantOffer,
                TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            _logger.LogInformation("Restaurant Offer { stayId } updated successfully", restaurantOffer.Id);
            return restaurantOffer.ToDto();
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task Delete(Guid id) {
        try {
            var restaurantOffer = await _restaurantOfferDataAccess.GetById(id);
            if (restaurantOffer is null) {
                var errorMessage = "Restaurant offer not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            await _restaurantOfferDataAccess.Delete(restaurantOffer);

            // Remove from cache
            RemoveCache(restaurantOffer.Id, restaurantOffer.UserId);

            _logger.LogInformation("Restaurant Offer { id } deleted successfully", restaurantOffer.Id);
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    private void RemoveCache(Guid restaurantOfferId, Guid userId) {
        // Remove from cache
        var cacheKey = Consts.RestaurantOffersCacheKey.Concat(restaurantOfferId.ToString());
        if (_cache.TryGetValue(cacheKey, out RestaurantOffer cachedRestaurantOffer)) _cache.Remove(cacheKey);
        // Remove from cache
        var userCacheKey = Consts.RestaurantOffersUserIdCacheKey.Concat(userId.ToString());
        if (_cache.TryGetValue(userCacheKey, out IEnumerable<RestaurantOfferDto> cachedRestaurantOffers))
            _cache.Remove(userCacheKey);
    }
}