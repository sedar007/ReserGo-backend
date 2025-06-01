using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Requests.Products.Hotel.Rooms;
using ReserGo.Common.Response;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;

namespace ReserGo.Business.Implementations;

public class RoomAvailabilityService : IRoomAvailabilityService {
    private readonly IRoomAvailabilityDataAccess _availabilityDataAccess;
    private readonly IMemoryCache _cache;
    private readonly IHotelService _hotelService;
    private readonly IImageService _imageService;
    private readonly ILogger<UserService> _logger;

    private readonly IRoomDataAccess _roomDataAccess;

    public RoomAvailabilityService(ILogger<UserService> logger, IRoomDataAccess roomDataAccess,
        IRoomAvailabilityDataAccess availabilityDataAccess, IHotelService hotelService, IMemoryCache cache,
        IImageService imageService) {
        _logger = logger;
        _roomDataAccess = roomDataAccess;
        _availabilityDataAccess = availabilityDataAccess;
        _hotelService = hotelService;
        _imageService = imageService;
        _cache = cache;
    }


    public async Task<RoomAvailabilityDto> SetAvailability(ConnectedUser connectedUser, Guid roomId,
        RoomAvailabilityRequest request) {
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

        DeleteCache(connectedUser.UserId);
        _logger.LogInformation("Availability successfully processed for RoomId: {RoomId}", roomId);
        return availabilityResponse.ToDto();
    }

    public async Task<RoomAvailabilityDto> GetAvailabilityByRoomId(Guid roomId, DateOnly startDate, DateOnly endDate) {
        var resultList = (await _availabilityDataAccess.GetAvailabilitiesByRoomIdDate(roomId, startDate, endDate))
            .ToList();

        if (!resultList.Any()) {
            _logger.LogWarning("No availability found for RoomId: {RoomId} between {StartDate} and {EndDate}",
                roomId, startDate, endDate);
            throw new InvalidDataException("No availability found for the specified room and dates.");
        }

        var availableRooms = resultList
            .Where(a => a.BookingsHotels.All(b => b.EndDate <= startDate || b.StartDate >= endDate)).ToList();

        if (!availableRooms.Any()) {
            _logger.LogWarning("No available rooms found for RoomId: {RoomId} between {StartDate} and {EndDate}",
                roomId, startDate, endDate);
            throw new InvalidDataException("No available rooms found for the specified dates.");
        }

        return availableRooms.FirstOrDefault()?.ToDto() ??
               throw new InvalidDataException("No available rooms found for the specified dates.");
    }

    public async Task<IEnumerable<RoomAvailabilityDto>> GetAvailabilitiesByHotelId(ConnectedUser connectedUser,
        Guid hotelId, int skip, int take) {
        IsAuthorized(connectedUser, hotelId);

        _logger.LogInformation("Fetching room availabilities for HotelId: {HotelId}", hotelId);
        var availabilities = await _availabilityDataAccess.GetAvailabilitiesByHotelId(hotelId, skip, take);
        var availabilityDtos = availabilities.Select(a => a.ToDto()).ToList();

        return availabilityDtos;
    }

    public async Task<IEnumerable<RoomAvailabilityDto>> GetAvailabilitiesForAllHotels(ConnectedUser connectedUser,
        int skip, int take) {
        Utils.IsAuthorized(connectedUser, _logger);

        var cacheKey = string.Format(Consts.CacheKeyAvailabilitiesRoom, connectedUser.UserId, skip, take);
        if (_cache.TryGetValue(cacheKey, out IEnumerable<RoomAvailabilityDto>? cachedAvailabilities))
            if (cachedAvailabilities != null) {
                _logger.LogInformation("Returning cached availabilities for UserId: {UserId}", connectedUser.UserId);
                return cachedAvailabilities;
            }

        _logger.LogInformation("Fetching hotels for UserId: {UserId}", connectedUser.UserId);
        var hotels = await _hotelService.GetHotelsByUserId(connectedUser.UserId);
        var hotelIds = hotels.Select(h => h.Id);

        _logger.LogInformation("Fetching availabilities for UserId: {UserId}", connectedUser.UserId);
        var availabilities = await _availabilityDataAccess.GetAvailabilitiesByHotelIds(hotelIds, skip, take);
        var availabilityDtos = availabilities.Select(a => a.ToDto()).ToList();

        _cache.Set(cacheKey, availabilityDtos, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
        _logger.LogInformation("Availabilities cached for UserId: {UserId}", connectedUser.UserId);

        return availabilityDtos;
    }


    public async Task<IEnumerable<RoomAvailibilityHotelResponse>> SearchAvailability(
        HotelSearchAvailabilityRequest request) {
        var result = (await _availabilityDataAccess.GetAvailability(request)).ToList();
        if (!result.Any()) {
            _logger.LogWarning("No availabilities found for the given search criteria.");
            return new List<RoomAvailibilityHotelResponse>();
        }

        var response = await Task.WhenAll(result
            .GroupBy(a => a.HotelId)
            .Select(async group => new RoomAvailibilityHotelResponse {
                HotelId = group.Key,
                HotelName = group.First().Hotel.Name,
                ImageSrc = await _imageService.GetPicture(group.First().Hotel.Picture ?? string.Empty),
                Rooms = await Task.WhenAll(group
                    .Where(a => a.BookingsHotels.All(b =>
                        b.EndDate <= request.ArrivalDate || b.StartDate >= request.ReturnDate))
                    .Select(async a => await Task.FromResult(a.Room.ToDto())))
            })
        );

        return response.Where(r => r.Rooms.Any());
    }

    private async void IsAuthorized(ConnectedUser connectedUser, Guid hotelId) {
        Utils.IsAuthorized(connectedUser, _logger);
        if (await _hotelService.IsAuthorized(hotelId, connectedUser.UserId)) return;

        _logger.LogWarning("User not authorized to set availability for this hotel.");
        throw new UnauthorizedAccessException("User not authorized to set availability for this hotel.");
    }

    private void ValidateRequest(RoomAvailabilityRequest request) {
        if (request.StartDate >= request.EndDate) {
            _logger.LogWarning("Start date must be before end date. StartDate: {StartDate}, EndDate: {EndDate}",
                request.StartDate, request.EndDate);
            throw new InvalidDataException("Start date must be before end date.");
        }

        if (request.StartDate >= DateOnly.FromDateTime(DateTime.UtcNow)) return;

        _logger.LogWarning("Start date cannot be before today. StartDate: {StartDate}", request.StartDate);
        throw new InvalidDataException("Start date cannot be before today.");
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

    private void DeleteCache(Guid userId) {
        const int skip = 0;
        const int take = 100;

        var cacheKeys = new[] {
            Consts.CacheKeyAvailabilitiesHotel,
            string.Format(Consts.CacheKeyAvailabilitiesRoom, userId, skip, take),
            Consts.CacheKeyAvailabilitiesRoom
        };

        foreach (var cacheKey in cacheKeys) {
            _cache.Remove(cacheKey);
            _logger.LogInformation("Cache removed for key: {CacheKey}", cacheKey);
        }
    }
}