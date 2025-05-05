using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;
using ReserGo.Shared.Interfaces;

namespace ReserGo.Business.Implementations;

public class RestaurantService : IRestaurantService {
    
    private readonly ISecurity _security;
    private readonly IMemoryCache _cache;
    private readonly ILogger<UserService> _logger;
    private readonly IImageService _imageService;
    private readonly IRestaurantDataAccess _restaurantDataAccess;
    
    public RestaurantService(IMemoryCache cache, ILogger<UserService> logger, IRestaurantDataAccess restaurantDataAccess, ISecurity security, IImageService imageService) {
        _cache = cache;
        _logger = logger;
        _security = security;
        _imageService = imageService;
        _restaurantDataAccess = restaurantDataAccess;
    }
    
    public async Task<RestaurantDto> Create(RestaurantCreationRequest request) {
        try {
            Restaurant? restaurant = await _restaurantDataAccess.GetByStayId(request.StayId);
            if (restaurant is not null) {
                string errorMessage = "This restaurant already exists.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }
            
            string error = RestaurantValidator.GetError(request);
            if (!string.IsNullOrEmpty(error)) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            ConnectedUser connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) throw new UnauthorizedAccessException("User not connected");
            
            Restaurant newRestaurant = new Restaurant {
                Name = request.Name,
                Capacity = request.Capacity,
                CuisineType = request.CuisineType,
                StayId = request.StayId,
                UserId = connectedUser.UserId,
                Location = request.Location,
                Picture = request.Picture != null ? await _imageService.UploadImage(request.Picture, connectedUser.UserId) : null
            };
            
            newRestaurant = await _restaurantDataAccess.Create(newRestaurant);
            _logger.LogInformation("Restaurant { id } created", newRestaurant.Id);
            return newRestaurant.ToDto();
            
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
    
    public async Task<RestaurantDto?> GetById(int id) {
        try {
            string cacheKey = $"Restaurant_GetById_{id}";

            if (_cache.TryGetValue(cacheKey, out RestaurantDto? cachedRestaurant)) {
                _logger.LogInformation("Returning cached restaurant for ID: {Id}", id);
                return cachedRestaurant;
            }

            Restaurant? restaurant = await _restaurantDataAccess.GetById(id);
            if (restaurant is null) {
                string errorMessage = "This restaurant does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            _logger.LogInformation("Restaurant {Id} retrieved successfully", restaurant.Id);
            RestaurantDto restaurantDto = restaurant.ToDto();
            _cache.Set(cacheKey, restaurantDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            return restaurantDto;
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
    
    public async Task<RestaurantDto?> GetByStayId(long stayId) {
        try {
            string cacheKey = $"Restaurant_GetByStayId_{stayId}";

            if (_cache.TryGetValue(cacheKey, out RestaurantDto? cachedRestaurant)) {
                _logger.LogInformation("Returning cached restaurant for StayId: {StayId}", stayId);
                return cachedRestaurant;
            }

            Restaurant? restaurant = await _restaurantDataAccess.GetByStayId(stayId);
            if (restaurant is null) {
                string errorMessage = "This restaurant does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            _logger.LogInformation("Restaurant {StayId} retrieved successfully", restaurant.StayId);
            RestaurantDto restaurantDto = restaurant.ToDto();
            _cache.Set(cacheKey, restaurantDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            return restaurantDto;
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<RestaurantDto> Update(long stayId, RestaurantUpdateRequest request) {
        try {
            Restaurant? restaurant = await _restaurantDataAccess.GetByStayId(stayId);
            if (restaurant is null) throw new Exception("Restaurant not found");

            string error = RestaurantValidator.GetError(request);
            if (!string.IsNullOrEmpty(error)) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            restaurant.Name = request.Name;
            restaurant.Capacity = request.Capacity;
            restaurant.CuisineType = request.CuisineType;
            restaurant.Location = request.Location;
            restaurant.LastUpdated = DateTime.UtcNow;

            if (request.Picture != null) {
                string? oldPublicId = restaurant.Picture;

                string? publicId = await _imageService.UploadImage(request.Picture, restaurant.UserId);
                if (string.IsNullOrEmpty(publicId)) {
                    _logger.LogWarning("Image upload failed for file: {FileName}", request.Picture.FileName);
                    throw new InvalidDataException("Image upload failed.");
                }

                if (oldPublicId is not null) {
                    bool deleteResult = await _imageService.DeleteImage(oldPublicId);
                    if (!deleteResult) {
                        _logger.LogWarning("Failed to delete old image with publicId: {PublicId}", oldPublicId);
                    }
                }

                restaurant.Picture = publicId;
            }

            _logger.LogInformation("Restaurant {StayId} updated successfully", restaurant.StayId);
            await _restaurantDataAccess.Update(restaurant);

            // Invalidate cache
            RemoveCache(restaurant.Id, restaurant.StayId);

            return restaurant.ToDto();
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<IEnumerable<RestaurantDto>> GetRestaurantsByUserId(int userId) {
        try {
            IEnumerable<Restaurant> restaurants = await _restaurantDataAccess.GetRestaurantsByUserId(userId);
            return restaurants.Select(hotel => hotel.ToDto());
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task Delete(int id) {
        try {
            Restaurant? restaurant = await _restaurantDataAccess.GetById(id);
            if (restaurant is null) {
                string errorMessage = "Restaurant not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            await _restaurantDataAccess.Delete(restaurant);

            // Invalidate cache
            RemoveCache(restaurant.Id, restaurant.StayId);

            _logger.LogInformation("Restaurant {Id} deleted successfully", restaurant.Id);
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    private void RemoveCache(int id, long stayId) {
        _cache.Remove($"Restaurant_GetById_{id}");
        _cache.Remove($"Restaurant_GetByStayId_{stayId}");
    }
}
