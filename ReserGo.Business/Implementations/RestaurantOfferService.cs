using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.Common.Response;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;
using ReserGo.Shared.Interfaces;
using InvalidDataException = System.IO.InvalidDataException;

namespace ReserGo.Business.Implementations;

public class RestaurantOfferService : IRestaurantOfferService {
    private readonly IMemoryCache _cache;
    private readonly IImageService _imageService;
    private readonly ILogger<RestaurantOfferService> _logger;
    private readonly IRestaurantOfferDataAccess _restaurantOfferDataAccess;
    private readonly IRestaurantService _restaurantService;
    private readonly ISecurity _security;

    public RestaurantOfferService(ILogger<RestaurantOfferService> logger,
        IRestaurantOfferDataAccess restaurantOfferDataAccess,
        IRestaurantService restaurantService,
        ISecurity security,
        IMemoryCache cache,
        IImageService imageService) {
        _logger = logger;
        _security = security;
        _restaurantOfferDataAccess = restaurantOfferDataAccess;
        _restaurantService = restaurantService;
        _cache = cache;
        _imageService = imageService;
    }

    public async Task<RestaurantOfferDto> Create(RestaurantOfferCreationRequest request) {
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

            if (request.PricePerPerson < 0) {
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

            DeleteCache(newRestaurantOffer.Id, connectedUser.UserId);
            // Cache the created restaurant offer
            _cache.Set(cacheKey, newRestaurantOffer,
                TimeSpan.FromMinutes(Consts.CacheDurationMinutes));


            _logger.LogInformation("Restaurant Offer {Id} created", newRestaurantOffer.Id);
            return newRestaurantOffer.ToDto();
        
    }

    public async Task<IEnumerable<RestaurantAvailabilityResponse>> SearchAvailability(
        RestaurantSearchAvailabilityRequest request) {
            var result = (await _restaurantOfferDataAccess.SearchAvailability(request)).ToList();
            if (!result.Any()) {
                _logger.LogWarning("No restaurant offers found for the given search criteria.");
                return new List<RestaurantAvailabilityResponse>();
            }

            var availableOffers = result
                .Where(o => o.GuestLimit - o.GuestNumber >= request.NumberOfGuests)
                .Select(o => new RestaurantAvailabilityResponse {
                    RestaurantOfferId = o.Id,
                    TypeOfCuisine = o.Restaurant.CuisineType,
                    RestaurantName = o.Restaurant.Name,
                    PricePerGuest = o.PricePerPerson,
                    AvailableCapacity = o.GuestLimit - o.GuestNumber,
                    ImageSrc = _imageService.GetPicture(o.Restaurant.Picture ?? " ").Result
                });

            return availableOffers;
        
    }

    public async Task<RestaurantOfferDto?> GetById(Guid id) {
            if (id == Guid.Empty) {
                var errorMessage = "Invalid restaurant offer id";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var cacheKey = string.Format(Consts.RestaurantOffersCacheKey, id);

            if (_cache.TryGetValue(cacheKey, out RestaurantOfferDto? cachedRestaurantOffer))
                if (cachedRestaurantOffer != null)
                    return cachedRestaurantOffer;

            var restaurantOffer = await _restaurantOfferDataAccess.GetById(id);
            if (restaurantOffer is null) {
                var errorMessage = "This restaurant offer does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var response = restaurantOffer.ToDto();
            _cache.Set(cacheKey, response, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            _logger.LogInformation("Restaurant Offer {Id} retrieved successfully", restaurantOffer.Id);
            return response;
        
    }

    public async Task<IEnumerable<RestaurantOfferDto>> GetRestaurantsByUserId(Guid userId) {
            var cacheKey = string.Format(Consts.RestaurantOffersUserIdCacheKey, userId);

            if (_cache.TryGetValue(cacheKey, out IEnumerable<RestaurantOfferDto>? cachedRestaurantOffers))
                if (cachedRestaurantOffers != null)
                    return cachedRestaurantOffers;

            var restaurantOffers = (await _restaurantOfferDataAccess.GetRestaurantsOfferByUserId(userId)).ToList();
            if (!restaurantOffers.Any()) {
                _logger.LogWarning("No restaurant offers found for user {UserId}", userId);
                return Enumerable.Empty<RestaurantOfferDto>();
            }

            var restaurantOfferDtos =
                restaurantOffers.Select(restaurantOffer => restaurantOffer.ToDto()).ToList();

            _cache.Set(cacheKey, restaurantOfferDtos.ToList(), TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            return restaurantOfferDtos;
        
    }

    public async Task<RestaurantOfferDto> Update(Guid id, RestaurantOfferUpdateRequest request) {
            var restaurantOffer = await _restaurantOfferDataAccess.GetById(id);
            if (restaurantOffer is null) throw new InvalidDataException("Restaurant offer not found");

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

            _logger.LogInformation("Restaurant Offer {StayId} updated successfully", restaurantOffer.Id);
            return restaurantOffer.ToDto();
        
    }

    public async Task<RestaurantOfferDto> Update(RestaurantOfferDto restaurantOfferDto) {
            var restaurantOffer = await _restaurantOfferDataAccess.GetById(restaurantOfferDto.Id);
            if (restaurantOffer is null) throw new InvalidDataException("restaurantOffer not found");

            restaurantOffer.Description = restaurantOfferDto.Description;
            restaurantOffer.PricePerPerson = restaurantOfferDto.PricePerPerson;
            restaurantOffer.GuestLimit = restaurantOfferDto.GuestLimit;
            restaurantOffer.GuestNumber = restaurantOfferDto.GuestNumber;
            restaurantOffer.OfferStartDate = restaurantOfferDto.OfferStartDate;
            restaurantOffer.OfferEndDate = restaurantOfferDto.OfferEndDate;
            restaurantOffer.IsActive = restaurantOfferDto.IsActive;

            restaurantOffer = await _restaurantOfferDataAccess.Update(restaurantOffer);

            _logger.LogInformation("Restaurant Offer {StayId} updated successfully", restaurantOffer.Id);
            return restaurantOffer.ToDto();
        
    }

    public async Task Delete(Guid id) {
       
            var restaurantOffer = await _restaurantOfferDataAccess.GetById(id);
            if (restaurantOffer is null) {
                var errorMessage = "Restaurant offer not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            await _restaurantOfferDataAccess.Delete(restaurantOffer);

            // Remove from cache
            DeleteCache(restaurantOffer.Id, restaurantOffer.UserId);

            _logger.LogInformation("Restaurant Offer {Id} deleted successfully", restaurantOffer.Id);
        
    }

    private void DeleteCache(Guid restaurantOfferId, Guid userId) {
        var cacheKeys = new[] {
            string.Format(Consts.RestaurantOffersCacheKey, restaurantOfferId),
            string.Format(Consts.RestaurantOffersUserIdCacheKey, userId)
        };

        foreach (var cacheKey in cacheKeys) {
            _cache.Remove(cacheKey);
            _logger.LogInformation("Cache removed for key: {CacheKey}", cacheKey);
        }
    }
}