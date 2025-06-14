using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products.Hotel.Rooms;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;

namespace ReserGo.Business.Implementations;

public class RoomService : IRoomService {
    private readonly IMemoryCache _cache;
    private readonly IHotelService _hotelService;
    private readonly ILogger<RoomService> _logger;
    private readonly IRoomDataAccess _roomDataAccess;

    public RoomService(ILogger<RoomService> logger, IRoomDataAccess roomDataAccess,
        IMemoryCache cache, IHotelService hotelService) {
        _logger = logger;
        _roomDataAccess = roomDataAccess;
        _cache = cache;
        _hotelService = hotelService;
    }

    public async Task<RoomDto> Create(RoomCreationRequest request) {
        var error = RoomValidator.GetError(request);
        if (!string.IsNullOrEmpty(error)) {
            _logger.LogError(error);
            throw new InvalidDataException(error);
        }

        var newRoom = new Room {
            RoomNumber = request.RoomNumber,
            Capacity = request.Capacity,
            PricePerNight = request.PricePerNight,
            IsAvailable = request.IsAvailable,
            HotelId = request.HotelId
        };

        newRoom = await _roomDataAccess.Create(newRoom);

        var roomDto = newRoom.ToDto();
        _cache.Set($"GetRoomById_{newRoom.Id}", roomDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
        _logger.LogInformation("Room {Id} created successfully", newRoom.Id);
        return roomDto;
    }

    public async Task<RoomDto?> GetById(Guid id) {
        var cacheKey = $"room_{id}";

        if (_cache.TryGetValue(cacheKey, out RoomDto? cachedRoom)) {
            _logger.LogInformation("Returning cached room for ID: {Id}", id);
            return cachedRoom;
        }

        var room = await _roomDataAccess.GetById(id);
        if (room is null) {
            var errorMessage = "This room does not exist.";
            _logger.LogError(errorMessage);
            throw new InvalidDataException(errorMessage);
        }

        var roomDto = room.ToDto();
        _cache.Set(cacheKey, roomDto, TimeSpan.FromMinutes(10));
        _logger.LogInformation("Room {Id} retrieved successfully", room.Id);
        return roomDto;
    }

    public async Task<IEnumerable<RoomDto>> GetRoomsByHotelId(Guid hotelId) {
        var cacheKey = $"rooms_user_{hotelId}";

        var hotel = await _hotelService.GetById(hotelId);
        if (hotel is null) {
            var errorMessage = "This hotel does not exist.";
            _logger.LogError(errorMessage);
            throw new InvalidDataException(errorMessage);
        }

        if (_cache.TryGetValue(cacheKey, out IEnumerable<RoomDto>? cachedRooms)) {
            _logger.LogInformation("Returning cached rooms for user ID: {HotelId}", hotelId);
            return cachedRooms;
        }

        var rooms = await _roomDataAccess.GetRoomsByHotelId(hotelId);
        var roomDtos = rooms.Select(room => room.ToDto()).ToList();

        _cache.Set(cacheKey, roomDtos, TimeSpan.FromMinutes(10));
        _logger.LogInformation("Rooms for Hotel {HotelId} retrieved successfully", hotelId);
        return roomDtos;
    }


    public async Task<RoomDto> Update(Guid id, RoomUpdateRequest request) {
        var room = await _roomDataAccess.GetById(id);
        if (room is null) {
            var errorMessage = "Room not found.";
            _logger.LogError(errorMessage);
            throw new InvalidDataException(errorMessage);
        }

        var error = RoomValidator.GetError(request);
        if (!string.IsNullOrEmpty(error)) {
            _logger.LogError(error);
            throw new InvalidDataException(error);
        }

        room.RoomNumber = request.RoomNumber;
        room.Capacity = request.Capacity;
        room.PricePerNight = request.PricePerNight;
        room.IsAvailable = request.IsAvailable;

        await _roomDataAccess.Update(room);

        // Update cache
        var roomDto = room.ToDto();
        _cache.Set($"room_{room.Id}", roomDto, TimeSpan.FromMinutes(10));

        _logger.LogInformation("Room {Id} updated successfully", room.Id);
        return roomDto;
    }


    public async Task Delete(Guid id) {
        var room = await _roomDataAccess.GetById(id);
        if (room is null) {
            var errorMessage = "Room not found";
            _logger.LogError(errorMessage);
            throw new InvalidDataException(errorMessage);
        }

        // Delete image if it exists

        await _roomDataAccess.Delete(room);

        // Remove from cache
        _cache.Remove($"room_{room.Id}");
        _cache.Remove($"room_stay_{room.Id}");
        _cache.Remove($"rooms_user_{room.Id}");

        _logger.LogInformation("Room {Id} deleted successfully", room.Id);
    }
}