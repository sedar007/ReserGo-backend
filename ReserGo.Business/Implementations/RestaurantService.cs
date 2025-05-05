using Microsoft.Extensions.Logging;

using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared.Interfaces;

namespace ReserGo.Business.Implementations;
public class RestaurantService : IRestaurantService {
    
    private readonly ILogger<UserService> _logger;
    private readonly ISecurity _security;
    private readonly IImageService _imageService;
    private readonly IRestaurantDataAccess _restaurantDataAccess;
    
    public RestaurantService(ILogger<UserService> logger, IRestaurantDataAccess restaurantDataAccess, ISecurity security, IImageService imageService) {
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
            if (string.IsNullOrEmpty(error) == false) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            ConnectedUser connectedUser = _security.GetCurrentUser();
            if(connectedUser == null) throw new UnauthorizedAccessException("User not connected");
            
            Restaurant newRestaurant = new Restaurant {
               Name = request.Name,
               Capacity = request.Capacity,
               CuisineType = request.CuisineType,
               StayId = request.StayId,
               UserId = connectedUser.UserId,
               Picture = (request.File != null) ? await _imageService.UploadImage(request.File, connectedUser.UserId):  null
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
            Restaurant? restaurant = await _restaurantDataAccess.GetById(id);
            if (restaurant is null) {
                string errorMessage = "This restaurant does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }
            
            _logger.LogInformation("Restaurant { Id } retrieved successfully", restaurant.Id);
            RestaurantDto restaurantDto = restaurant.ToDto();
            return restaurantDto;
            
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
    
    public async Task<RestaurantDto?> GetByStayId(long stayId) {
        try {
            Restaurant? restaurant = await _restaurantDataAccess.GetByStayId(stayId);
            if (restaurant is null) {
                string errorMessage = "This restaurant does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }
            
            _logger.LogInformation("Restaurant { stayId } retrieved successfully", restaurant.StayId);
            RestaurantDto restaurantDto = restaurant.ToDto();
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
            if (string.IsNullOrEmpty(error) == false) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            restaurant.Name = request.Name;
            restaurant.Capacity = request.Capacity;
            restaurant.CuisineType = request.CuisineType;
            restaurant.LastUpdated = DateTime.UtcNow;

            if (request.File != null) {
                string? oldPublicId = restaurant.Picture;
            
                string? publicId = await _imageService.UploadImage(request.File, restaurant.UserId);
                if(string.IsNullOrEmpty(publicId)) {
                    _logger.LogWarning("Image upload failed for file: {FileName}", request.File.FileName);
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
            
            _logger.LogInformation("Restaurant { stayId } updated successfully", restaurant.StayId);
            await _restaurantDataAccess.Update(restaurant);
            return restaurant.ToDto();
            
        }catch (Exception e) {
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
            _logger.LogInformation("Restaurant { id } deleted successfully", restaurant.Id);
            
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}