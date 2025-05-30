using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;
using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Enum;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.User;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;
using ReserGo.Common.Requests.Products.Hotel.Rooms;
using ReserGo.Common.Security;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Response;
namespace ReserGo.Business.Implementations;


public class RoomAvailabilityService : IRoomAvailabilityService {
    private readonly ILogger<UserService> _logger;

    private readonly IRoomDataAccess _roomDataAccess;
    private readonly IRoomAvailabilityDataAccess _availabilityDataAccess;
    private readonly IHotelService _hotelService;
    private readonly IImageService _imageService;
    // private readonly IMemoryCache _cache;

    public RoomAvailabilityService(ILogger<UserService> logger, IRoomDataAccess roomDataAccess,
        IRoomAvailabilityDataAccess availabilityDataAccess, IHotelService hotelService, IMemoryCache cache,
        IImageService imageService) {
        _logger = logger;
        _roomDataAccess = roomDataAccess;
        _availabilityDataAccess = availabilityDataAccess;
        _hotelService = hotelService;
        _imageService = imageService;
        // _cache = cache;
    }

    private async void IsAuthorized(ConnectedUser connectedUser, Guid hotelId) {
        IsAuthorized(connectedUser);
        if (!await _hotelService.IsAuthorized(hotelId, connectedUser.UserId)) {
            _logger.LogWarning("User not authorized to set availability for this hotel.");
            throw new UnauthorizedAccessException("User not authorized to set availability for this hotel.");
        }
    }

    private void IsAuthorized(ConnectedUser connectedUser) {
        if (connectedUser == null) {
            _logger.LogWarning("User not connected.");
            throw new UnauthorizedAccessException("User not connected.");
        }
    }

    public async Task<RoomAvailabilityDto> SetAvailability(ConnectedUser connectedUser, Guid roomId,
        RoomAvailabilityRequest request) {
        try {
            IsAuthorized(connectedUser, request.HotelId);

            ValidateRequest(request);

            var room = await _roomDataAccess.GetById(roomId);
            if (room == null) {
                _logger.LogWarning("Room not found for RoomId: {RoomId}", roomId);
                throw new InvalidDataException("Room not found.");
            }

            var existingAvailability = await _availabilityDataAccess.GetByRoomId(roomId);
            RoomAvailability availabilityResponse;

            if (existingAvailability == null || existingAvailability.EndDate < DateOnly.FromDateTime(DateTime.UtcNow))
                availabilityResponse = await CreateNewAvailability(roomId, request);
            else
                availabilityResponse = await ExtendExistingAvailability(existingAvailability, request);

            DeleteCache();
            _logger.LogInformation("Availability successfully processed for RoomId: {RoomId}", roomId);

            return availabilityResponse.ToDto();
        }
        catch (InvalidDataException ex) {
            _logger.LogError(ex, "Validation error while setting availability for RoomId: {RoomId}", roomId);
            throw;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An unexpected error occurred while setting availability for RoomId: {RoomId}",
                roomId);
            throw;
        }
    }

    public async Task<RoomAvailabilityDto> GetAvailabilityByRoomId(Guid roomId) {
        try {
            //  var cacheKey = $"availability_{id}";

            /*  if (_cache.TryGetValue(cacheKey, out RoomAvailabilityDto? cachedAvailability)) {
                  _logger.LogInformation("Returning cached availability for ID: {Id}", id);
                  return cachedAvailability;
              }*/

            var availability = await _availabilityDataAccess.GetByRoomId(roomId);
            if (availability == null) {
                var errorMessage = "This availability does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var availabilityDto = availability.ToDto();
            // _cache.Set(cacheKey, availabilityDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            _logger.LogInformation("Availability { id } retrieved successfully", roomId);

            return availabilityDto;
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    private void ValidateRequest(RoomAvailabilityRequest request) {
        if (request.StartDate >= request.EndDate) {
            _logger.LogWarning("Start date must be before end date. StartDate: {StartDate}, EndDate: {EndDate}",
                request.StartDate, request.EndDate);
            throw new InvalidDataException("Start date must be before end date.");
        }

        if (request.StartDate < DateOnly.FromDateTime(DateTime.UtcNow)) {
            _logger.LogWarning("Start date cannot be before today. StartDate: {StartDate}", request.StartDate);
            throw new InvalidDataException("Start date cannot be before today.");
        }
    }

    private async Task<RoomAvailability> CreateNewAvailability(Guid roomId, RoomAvailabilityRequest request) {
        _logger.LogInformation("Creating new availability for RoomId: {RoomId}", roomId);

        var availability = new RoomAvailability {
            RoomId = roomId,
            HotelId = request.HotelId,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        var createdAvailability = await _availabilityDataAccess.Create(availability);
        if (createdAvailability == null) {
            _logger.LogWarning("Failed to create availability for RoomId: {RoomId}", roomId);
            throw new InvalidDataException("Failed to create availability.");
        }

        return createdAvailability;
    }

    private async Task<RoomAvailability> ExtendExistingAvailability(RoomAvailability existingAvailability,
        RoomAvailabilityRequest request) {
        _logger.LogInformation("Extending existing availability for RoomId: {RoomId}", existingAvailability.RoomId);

        existingAvailability.StartDate = request.StartDate < existingAvailability.StartDate
            ? request.StartDate
            : existingAvailability.StartDate;

        existingAvailability.EndDate = request.EndDate > existingAvailability.EndDate
            ? request.EndDate
            : existingAvailability.EndDate;

        var updatedAvailability = await _availabilityDataAccess.Update(existingAvailability);
        if (updatedAvailability == null) {
            _logger.LogWarning("Failed to update availability for RoomId: {RoomId}", existingAvailability.RoomId);
            throw new InvalidDataException("Failed to update availability.");
        }

        return updatedAvailability;
    }

    public async Task<IEnumerable<RoomAvailabilityDto>> GetAvailabilitiesByHotelId(ConnectedUser connectedUser,
        Guid hotelId, int skip, int take) {
        try {
            IsAuthorized(connectedUser, hotelId);

            /* var cacheKey = string.Format(Consts.CacheKeyAvailabilitiesHotel, hotelId, skip, take);
             if (_cache.TryGetValue(cacheKey, out IEnumerable<RoomAvailabilityDto>? cachedAvailabilities)) {
                 _logger.LogInformation("Returning cached availabilities for HotelId: {HotelId}", hotelId);
                 return cachedAvailabilities;
             }*/

            _logger.LogInformation("Fetching room availabilities for HotelId: {HotelId}", hotelId);
            var availabilities = await _availabilityDataAccess.GetAvailabilitiesByHotelId(hotelId, skip, take);
            var availabilityDtos = availabilities.Select(a => a.ToDto()).ToList();

            //  _cache.Set(cacheKey, availabilityDtos, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            _logger.LogInformation("Availabilities cached for HotelId: {HotelId}", hotelId);

            return availabilityDtos;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving room availabilities for HotelId: {HotelId}",
                hotelId);
            throw;
        }
    }

    public async Task<IEnumerable<RoomAvailabilityDto>> GetAvailabilitiesForAllHotels(ConnectedUser connectedUser,
        int skip, int take) {
        try {
            IsAuthorized(connectedUser);

            /* var cacheKey = string.Format(Consts.CacheKeyAvailabilitiesUser, connectedUser.UserId, skip, take);
             if (_cache.TryGetValue(cacheKey, out IEnumerable<RoomAvailabilityDto>? cachedAvailabilities)) {
                 _logger.LogInformation("Returning cached availabilities for UserId: {UserId}", connectedUser.UserId);
                 return cachedAvailabilities;
             }*/

            _logger.LogInformation("Fetching hotels for UserId: {UserId}", connectedUser.UserId);
            var hotels = await _hotelService.GetHotelsByUserId(connectedUser.UserId);
            var hotelIds = hotels.Select(h => h.Id);

            _logger.LogInformation("Fetching availabilities for UserId: {UserId}", connectedUser.UserId);
            var availabilities = await _availabilityDataAccess.GetAvailabilitiesByHotelIds(hotelIds, skip, take);
            var availabilityDtos = availabilities.Select(a => a.ToDto()).ToList();

            // _cache.Set(cacheKey, availabilityDtos, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            _logger.LogInformation("Availabilities cached for UserId: {UserId}", connectedUser.UserId);

            return availabilityDtos;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving availabilities for UserId: {UserId}",
                connectedUser.UserId);
            throw;
        }
    }
    

    public async Task<IEnumerable<RoomAvailibilityHotelResponse>> SearchAvailability(HotelSearchAvailabilityRequest request) {
        try {
            var result = await _availabilityDataAccess.GetAvailability(request);
            if (result == null || !result.Any()) {
                _logger.LogWarning("No availabilities found for the given search criteria.");
                return new List<RoomAvailibilityHotelResponse>();
            }

            var resultList = result.ToList();

            var availableRooms = resultList
                .Where(a => a.BookingsHotels.All(b => b.EndDate <= request.ArrivalDate || b.StartDate >= request.ReturnDate))
                .Select(async a => new RoomAvailibilityHotelResponse {
                    RoomId = a.RoomId,
                    HotelId = a.HotelId,
                    PricePerNightPerPerson = a.Room.PricePerNight,
                    HotelName = a.Hotel.Name,
                    RoomName = a.Room.RoomNumber,
                    NumberOfGuests = a.Room.Capacity,
                    ImageSrc = await _imageService.GetPicture(a.Hotel.Picture ?? " ")
                });

            return await Task.WhenAll(availableRooms);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while searching availability.");
            throw;
        }
    }

    private void DeleteCache() {
        var cacheKeys = new[] {
            Consts.CacheKeyAvailabilitiesHotel,
            Consts.CacheKeyAvailabilitiesUser
        };

        /* foreach (var keyTemplate in cacheKeys) {
             for (int skip = 0; skip <= 100; skip += 10) { // Adjust range/step as needed
                 var cacheKey = string.Format(keyTemplate, Guid.Empty, skip, 10);
                 _cache.Remove(cacheKey);
             }
         }*/
    }
}