using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;
using ReserGo.Shared.Interfaces;

namespace ReserGo.Business.Implementations;

public class HotelService : IHotelService {
    private readonly IMemoryCache _cache;
    private readonly IHotelDataAccess _hotelDataAccess;
    private readonly IImageService _imageService;
    private readonly ILogger<HotelService> _logger;
    private readonly IRoomDataAccess _roomDataAccess;
    private readonly ISecurity _security;

    public HotelService(ILogger<HotelService> logger, IHotelDataAccess hotelDataAccess, ISecurity security,
        IImageService imageService, IMemoryCache cache, IRoomDataAccess roomDataAccess) {
        _logger = logger;
        _security = security;
        _imageService = imageService;
        _hotelDataAccess = hotelDataAccess;
        _cache = cache;
        _roomDataAccess = roomDataAccess;
    }

    public async Task<HotelDto> Create(HotelCreationRequest request) {
        var hotel = await _hotelDataAccess.GetByStayId(request.StayId);
        if (hotel is not null) {
            var errorMessage = "This hotel already exists.";
            _logger.LogError(errorMessage);
            throw new InvalidDataException(errorMessage);
        }

        var error = HotelValidator.GetError(request);
        if (!string.IsNullOrEmpty(error)) {
            _logger.LogError(error);
            throw new InvalidDataException(error);
        }

        var connectedUser = _security.GetCurrentUser();
        if (connectedUser == null) throw new UnauthorizedAccessException("User not connected");

        var newHotel = new Hotel {
            Name = request.Name,
            Location = request.Location,
            StayId = request.StayId,
            Picture = request.Picture != null
                ? await _imageService.UploadImage(request.Picture, connectedUser.UserId)
                : null,
            UserId = connectedUser.UserId,
            LastUpdated = DateTime.UtcNow
        };

        newHotel = await _hotelDataAccess.Create(newHotel);

        var random = new Random();
        // Create Rooms
        var rooms = new List<Room>();
        for (var i = 1; i <= request.NbRoomsVip; i++)
            rooms.Add(new Room {
                RoomNumber = $"VIP-{i}",
                Capacity = random.Next(2, 5),
                PricePerNight = request.PriceVip,
                IsAvailable = true,
                HotelId = newHotel.Id
            });

        for (var i = 1; i <= request.NbRoomsStandard; i++)
            rooms.Add(new Room {
                RoomNumber = $"STD-{i}",
                Capacity = random.Next(1, 4),
                PricePerNight = request.PriceStandard,
                IsAvailable = true,
                HotelId = newHotel.Id
            });

        foreach (var room in rooms) await _roomDataAccess.Create(room);

        newHotel.NumberOfRooms = rooms.Count;
        newHotel = await _hotelDataAccess.Update(newHotel);

        var hotelDto = newHotel.ToDto();
        var cacheKey = string.Format(Consts.CacheKeyHotel, hotelDto.Id);
        var cacheKeyStayId = string.Format(Consts.CacheKeyHotelStayId, hotelDto.StayId);
    
        // Cache the created hotel
        _cache.Set(cacheKey, hotelDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
        _cache.Set(cacheKeyStayId, hotelDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

        _logger.LogInformation("Hotel {Id} created", newHotel.Id);
        return hotelDto;
    }

    public async Task<HotelDto?> GetById(Guid id) {
        var cacheKey = string.Format(Consts.CacheKeyHotel, id);
        if (_cache.TryGetValue(cacheKey, out HotelDto? cachedHotel))
            if (cachedHotel != null)
                return cachedHotel;

        var hotel = await _hotelDataAccess.GetById(id);
        if (hotel is null) {
            var errorMessage = "This hotel does not exist.";
            _logger.LogError(errorMessage);
            throw new InvalidDataException(errorMessage);
        }
        
        var hotelDto = hotel.ToDto();

        _cache.Set(cacheKey, hotelDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
        _logger.LogInformation("Hotel {Id} retrieved successfully", hotel.Id);
        return hotelDto;
    }

    public async Task<IEnumerable<HotelDto>> GetHotelsByUserId(Guid userId) {
        var hotels = (await _hotelDataAccess.GetHotelsByUserId(userId)).ToList();
        if (!hotels.Any()) {
            var errorMessage = "This user has no hotels.";
            _logger.LogError(errorMessage);
            return Enumerable.Empty<HotelDto>();
        }

        return hotels.Select(hotel => hotel.ToDto());
    }

    public async Task<HotelDto?> GetByStayId(long stayId) {
        var cacheKey = string.Format(Consts.CacheKeyHotelStayId, stayId);

        if (_cache.TryGetValue(cacheKey, out HotelDto? cachedHotel))
            if (cachedHotel != null)
                return cachedHotel;

        var hotel = await _hotelDataAccess.GetByStayId(stayId);
        if (hotel is null) {
            var errorMessage = "This hotel does not exist.";
            _logger.LogError(errorMessage);
            throw new InvalidDataException(errorMessage);
        }
        var hotelDto = hotel.ToDto();
        _cache.Set(cacheKey, hotelDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

        _logger.LogInformation("Hotel {StayId} retrieved successfully", hotel.StayId);
        return hotelDto;
    }

    public async Task<HotelDto> Update(long stayId, HotelUpdateRequest request) {
        var hotel = await _hotelDataAccess.GetByStayId(stayId);
        if (hotel is null) throw new InvalidDataException("Hotel not found");

        var error = HotelValidator.GetError(request);
        if (!string.IsNullOrEmpty(error)) {
            _logger.LogError(error);
            throw new InvalidDataException(error);
        }

        hotel.Name = request.Name;
        hotel.Location = request.Location;
        hotel.LastUpdated = DateTime.UtcNow;

        if (request.Picture != null) {
            var oldPublicId = hotel.Picture;

            var publicId = await _imageService.UploadImage(request.Picture, hotel.UserId);
            if (string.IsNullOrEmpty(publicId)) {
                _logger.LogWarning("Image upload failed for file: {FileName}", request.Picture.FileName);
                throw new InvalidDataException("Image upload failed.");
            }

            if (oldPublicId is not null) {
                var deleteResult = await _imageService.DeleteImage(oldPublicId);
                if (!deleteResult)
                    _logger.LogWarning("Failed to delete old image with publicId: {PublicId}", oldPublicId);
            }

            hotel.Picture = publicId;
        }

        await _hotelDataAccess.Update(hotel);

        var hotelDto = hotel.ToDto();
        // Update cache
        var cacheKey = string.Format(Consts.CacheKeyHotel, hotelDto.Id);
        var cacheKeyStayId = string.Format(Consts.CacheKeyHotelStayId, hotelDto.StayId);
        
        _cache.Set(cacheKey, hotelDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
        _cache.Set(cacheKeyStayId, hotelDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

        _logger.LogInformation("Hotel {StayId} updated successfully", hotelDto.StayId);
        return hotelDto;
    }

    public async Task Delete(Guid id) {
        var hotel = await _hotelDataAccess.GetById(id);
        if (hotel is null) {
            var errorMessage = "Hotel not found";
            _logger.LogError(errorMessage);
            throw new InvalidDataException(errorMessage);
        }

        // Delete image if it exists
        var publicId = hotel.Picture;
        await _hotelDataAccess.Delete(hotel);
        if (publicId is not null) {
            var deleteResult = await _imageService.DeleteImage(publicId);
            if (!deleteResult) _logger.LogWarning("Failed to delete image with publicId: {PublicId}", publicId);
        }

        var cacheKey = string.Format(Consts.CacheKeyHotel, hotel.Id);
        var cacheKeyStayId = string.Format(Consts.CacheKeyHotelStayId, hotel.StayId);

        // Remove from cache
        _cache.Remove(cacheKey);
        _cache.Remove(cacheKeyStayId);

        _logger.LogInformation("Hotel {Id} deleted successfully", hotel.Id);
    }

    public async Task<bool> IsAuthorized(Guid hotelId, Guid userId) {
        var isAuthorized = await _hotelDataAccess.IsAuthorized(hotelId, userId);
        if (!isAuthorized) {
            var errorMessage = "User is not authorized to access this hotel.";
            _logger.LogError(errorMessage);
            throw new UnauthorizedAccessException(errorMessage);
        }

        return true;
    }
}