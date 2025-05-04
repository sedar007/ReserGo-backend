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

    /*public async Task<HotelDto> Update(long stayId, HotelUpdateRequest request) {
        try {
            Hotel? hotel = await _hotelDataAccess.GetByStayId(stayId);
            if (hotel is null) throw new Exception("Hotel not found");

            string error = HotelValidator.GetError(request);
            if (string.IsNullOrEmpty(error) == false) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            hotel.Name = request.Name;
            hotel.Location = request.Location;
            hotel.Capacity = request.Capacity;
            hotel.LastUpdated = DateTime.UtcNow;

            if (request.File != null) {
                string? oldPublicId = hotel.Picture;
            
                string? publicId = await _imageService.UploadImage(request.File, hotel.UserId);
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
                hotel.Picture = publicId;
            }
            
            _logger.LogInformation("Hotel { stayId } updated successfully", hotel.StayId);
            await _hotelDataAccess.Update(hotel);
            return hotel.ToDto();
            
        }catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
    
    public async Task Delete(int id) {
        try {
            Hotel? hotel = await _hotelDataAccess.GetById(id);
            if (hotel is null) {
                string errorMessage = "Hotel not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }
            await _hotelDataAccess.Delete(hotel);
            _logger.LogInformation("Hotel { id } deleted successfully", hotel.Id);
            
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }*/
}