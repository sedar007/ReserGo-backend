using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products.Event;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;
using ReserGo.Shared.Interfaces;

namespace ReserGo.Business.Implementations;

public class EventService : IEventService {
    private readonly IMemoryCache _cache;
    private readonly IImageService _imageService;
    private readonly ILogger<EventService> _logger;
    private readonly IEventDataAccess _occasionDataAccess;
    private readonly ISecurity _security;

    public EventService(IMemoryCache cache, ILogger<EventService> logger, IEventDataAccess occasionDataAccess,
        ISecurity security, IImageService imageService) {
        _cache = cache;
        _logger = logger;
        _security = security;
        _imageService = imageService;
        _occasionDataAccess = occasionDataAccess;
    }

    public async Task<EventDto> Create(EventCreationRequest request) {
        
            var occasion = await _occasionDataAccess.GetByStayId(request.StayId);
            if (occasion is not null) {
                var errorMessage = "This @event already exists.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var error = EventValidator.GetError(request);
            if (!string.IsNullOrEmpty(error)) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) throw new UnauthorizedAccessException("User not connected");

            var newEvent = new Event {
                Name = request.Name,
                Location = request.Location,
                Capacity = request.Capacity,
                StayId = request.StayId,
                Picture = request.Picture != null
                    ? await _imageService.UploadImage(request.Picture, connectedUser.UserId)
                    : null,
                UserId = connectedUser.UserId,
                LastUpdated = DateTime.UtcNow
            };

            newEvent = await _occasionDataAccess.Create(newEvent);

            _logger.LogInformation("Event {Id} created", newEvent.Id);

            // Set cache
            _cache.Set($"Event_GetById_{newEvent.Id}", newEvent.ToDto(),
                TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            _cache.Set($"Event_GetByStayId_{newEvent.StayId}", newEvent.ToDto(),
                TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            return newEvent.ToDto();
        
    }

    public async Task<EventDto?> GetById(Guid id) {
       
            var cacheKey = $"Event_GetById_{id}";

            if (_cache.TryGetValue(cacheKey, out EventDto? cachedEvent)) {
                _logger.LogInformation("Returning cached @event for ID: {Id}", id);
                return cachedEvent;
            }

            var occasion = await _occasionDataAccess.GetById(id);
            if (occasion is null) {
                var errorMessage = "This @event does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            _logger.LogInformation("Event {Id} retrieved successfully", occasion.Id);
            var occasionDto = occasion.ToDto();
            _cache.Set(cacheKey, occasionDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            return occasionDto;
        
    }

    public async Task<EventDto?> GetByStayId(long stayId) {
       
            var cacheKey = $"Event_GetByStayId_{stayId}";

            if (_cache.TryGetValue(cacheKey, out EventDto? cachedEvent)) {
                _logger.LogInformation("Returning cached @event for StayId: {StayId}", stayId);
                return cachedEvent;
            }

            var occasion = await _occasionDataAccess.GetByStayId(stayId);
            if (occasion is null) {
                var errorMessage = "This @event does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            _logger.LogInformation("Event {StayId} retrieved successfully", occasion.StayId);
            var occasionDto = occasion.ToDto();
            _cache.Set(cacheKey, occasionDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            return occasionDto;
        
    }

    public async Task<IEnumerable<EventDto>> GetEventsByUserId(Guid userId) {
       
            var occasions = await _occasionDataAccess.GetEventsByUserId(userId);
            return occasions.Select(occasion => occasion.ToDto());
        
    }

    public async Task<EventDto> Update(long stayId, EventUpdateRequest request) {
       
            var occasion = await _occasionDataAccess.GetByStayId(stayId);
            if (occasion is null) throw new InvalidDataException("Event not found");

            var error = EventValidator.GetError(request);
            if (!string.IsNullOrEmpty(error)) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            occasion.Name = request.Name;
            occasion.Location = request.Location;
            occasion.Capacity = request.Capacity;
            occasion.LastUpdated = DateTime.UtcNow;

            if (request.Picture != null) {
                var oldPublicId = occasion.Picture;

                var publicId = await _imageService.UploadImage(request.Picture, occasion.UserId);
                if (string.IsNullOrEmpty(publicId)) {
                    _logger.LogWarning("Image upload failed for file: {FileName}", request.Picture.FileName);
                    throw new InvalidDataException("Image upload failed.");
                }

                if (oldPublicId is not null) {
                    var deleteResult = await _imageService.DeleteImage(oldPublicId);
                    if (!deleteResult)
                        _logger.LogWarning("Failed to delete old image with publicId: {PublicId}", oldPublicId);
                }

                occasion.Picture = publicId;
            }

            _logger.LogInformation("Event {StayId} updated successfully", occasion.StayId);
            await _occasionDataAccess.Update(occasion);

            // Invalidate cache
            RemoveCache(occasion.Id, occasion.StayId);

            return occasion.ToDto();
        
    }

    public async Task Delete(Guid id) {
        
            var occasion = await _occasionDataAccess.GetById(id);
            if (occasion is null) {
                var errorMessage = "Event not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var oldPublicId = occasion.Picture;
            await _occasionDataAccess.Delete(occasion);
            if (oldPublicId is not null) {
                var deleteResult = await _imageService.DeleteImage(oldPublicId);
                if (!deleteResult) _logger.LogWarning("Failed to delete image with publicId: {PublicId}", oldPublicId);
            }

            // Invalidate cache
            RemoveCache(occasion.Id, occasion.StayId);

            _logger.LogInformation("Event {Id} deleted successfully", occasion.Id);
        
    }

    private void RemoveCache(Guid id, long stayId) {
        _cache.Remove($"Event_GetById_{id}");
        _cache.Remove($"Event_GetByStayId_{stayId}");
    }
}