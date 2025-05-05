using Microsoft.Extensions.Logging;

using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared.Interfaces;

namespace ReserGo.Business.Implementations;
public class HotelService : IHotelService {
    
    private readonly ILogger<UserService> _logger;
    private readonly ISecurity _security;
    private readonly IImageService _imageService;
    private readonly IHotelDataAccess _hotelDataAccess;
    
    public HotelService(ILogger<UserService> logger, IHotelDataAccess hotelDataAccess, ISecurity security, IImageService imageService) {
        _logger = logger;
        _security = security;
        _imageService = imageService;
        _hotelDataAccess = hotelDataAccess;
    }
    
    public async Task<HotelDto> Create(HotelCreationRequest request) {
        try {
            Hotel? hotel = await _hotelDataAccess.GetByStayId(request.StayId);
            if (hotel is not null) {
                string errorMessage = "This hotel already exists.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }
            
            string error = HotelValidator.GetError(request);
            if (string.IsNullOrEmpty(error) == false) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            ConnectedUser connectedUser = _security.GetCurrentUser();
            if(connectedUser == null) throw new UnauthorizedAccessException("User not connected");
            
            Hotel newHotel = new Hotel {
                Name = request.Name,
                Location = request.Location,
                Capacity = request.Capacity,
                StayId = request.StayId,
                Picture = (request.Picture != null) ? await _imageService.UploadImage(request.Picture, connectedUser.UserId):  null,
                UserId = connectedUser.UserId
            };
            
            newHotel = await _hotelDataAccess.Create(newHotel);
            
            _logger.LogInformation("Hotel { id } created", newHotel.Id);
            return newHotel.ToDto();
            
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
    
    public async Task<HotelDto?> GetById(int id) {
        try {
            Hotel? hotel = await _hotelDataAccess.GetById(id);
            if (hotel is null) {
                string errorMessage = "This hotel does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }
            
            _logger.LogInformation("Hotel { Id } retrieved successfully", hotel.Id);
            HotelDto hotelDto = hotel.ToDto();
            return hotelDto;
            
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
    
    public async Task<HotelDto?> GetByStayId(long stayId) {
        try {
            Hotel? hotel = await _hotelDataAccess.GetByStayId(stayId);
            if (hotel is null) {
                string errorMessage = "This hotel does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }
            
            _logger.LogInformation("Hotel { stayId } retrieved successfully", hotel.StayId);
            HotelDto hotelDto = hotel.ToDto();
            return hotelDto;
            
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<HotelDto> Update(long stayId, HotelUpdateRequest request) {
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

            if (request.Picture != null) {
                string? oldPublicId = hotel.Picture;
            
                string? publicId = await _imageService.UploadImage(request.Picture, hotel.UserId);
                if(string.IsNullOrEmpty(publicId)) {
                    _logger.LogWarning("Image upload failed for file: {FileName}", request.Picture.FileName);
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
    }
}