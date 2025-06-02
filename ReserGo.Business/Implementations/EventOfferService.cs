using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products.Event;
using ReserGo.Common.Response;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;
using ReserGo.Shared.Exceptions;
using ReserGo.Shared.Interfaces;

namespace ReserGo.Business.Implementations;

public class EventOfferService : IEventOfferService {
    private readonly IMemoryCache _cache;
    private readonly IEventOfferDataAccess _eventOfferDataAccess;
    private readonly IImageService _imageService;
    private readonly ILogger<EventOfferService> _logger;
    private readonly IEventOfferDataAccess _occasionOfferDataAccess;
    private readonly IEventService _occasionService;
    private readonly ISecurity _security;

    public EventOfferService(ILogger<EventOfferService> logger, IEventOfferDataAccess occasionOfferDataAccess,
        IEventService occasionService, ISecurity security, IImageService imageService,
        IMemoryCache cache,
        IEventOfferDataAccess eventOfferDataAccess) {
        _logger = logger;
        _security = security;
        _imageService = imageService;
        _occasionOfferDataAccess = occasionOfferDataAccess;
        _occasionService = occasionService;
        _cache = cache;
        _eventOfferDataAccess = eventOfferDataAccess;
    }

    public async Task<EventOfferDto> Create(EventOfferCreationRequest request) {
        var error = EventOfferValidator.GetError(request);
        if (!string.IsNullOrEmpty(error)) {
            _logger.LogError(error);
            throw new InvalidDataException(error);
        }

        var connectedUser = _security.GetCurrentUser();
        if (connectedUser == null) throw new UnauthorizedAccessException("User not connected");

        var occasion = await _occasionService.GetById(request.EventId);
        if (occasion == null) {
            var errorMessage = "Event not found";
            _logger.LogError(errorMessage);
            throw new InvalidDataException(errorMessage);
        }

        var newEventOffer = new EventOffer {
            Description = request.Description,
            PricePerDay = request.PricePerDay,
            GuestLimit = request.GuestLimit,
            OfferStartDate = request.OfferStartDate,
            OfferEndDate = request.OfferEndDate,
            IsActive = request.IsActive,
            EventId = occasion.Id,
            UserId = connectedUser.UserId
        };

        newEventOffer = await _occasionOfferDataAccess.Create(newEventOffer);

        // Cache the created @event offer
        _cache.Set($"newEventOffer_{newEventOffer.Id}", newEventOffer,
            TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

        _logger.LogInformation("Event Offer {Id} created", newEventOffer.Id);
        return newEventOffer.ToDto();
    }

    public async Task<EventOfferDto?> GetById(Guid id) {
        if (_cache.TryGetValue($"occasionOffer_{id}", out EventOffer cachedEventOffer))
            return cachedEventOffer.ToDto();

        var occasionOffer = await _occasionOfferDataAccess.GetById(id);
        if (occasionOffer is null) {
            var errorMessage = "This @event offer does not exist.";
            _logger.LogError(errorMessage);
            throw new InvalidDataException(errorMessage);
        }

        _cache.Set($"occasionOffer_{id}", occasionOffer, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

        _logger.LogInformation("Event Offer {Id} retrieved successfully", occasionOffer.Id);
        return occasionOffer.ToDto();
    }

    public async Task<IEnumerable<EventOfferDto>> GetEventsByUserId(Guid userId) {
        var cacheKey = $"occasionOffers_user_{userId}";
        _cache.Remove($"occasionOffers_user_{userId}");

        if (_cache.TryGetValue(cacheKey, out IEnumerable<EventOfferDto> cachedEventOffers))
            return cachedEventOffers;

        var occasionOffers = await _occasionOfferDataAccess.GetEventsOfferByUserId(userId);
        var occasionOfferDtos =
            occasionOffers.Select(occasionOffer => occasionOffer.ToDto());

        _cache.Set(cacheKey, occasionOfferDtos, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

        return occasionOfferDtos;
    }

    public async Task<EventOfferDto> Update(Guid id, EventOfferUpdateRequest request) {
        var occasionOffer = await _occasionOfferDataAccess.GetById(id);
        if (occasionOffer is null) throw new NullDataException("Event offer not found");

        var error = EventOfferValidator.GetError(request);
        if (!string.IsNullOrEmpty(error)) {
            _logger.LogError(error);
            throw new InvalidDataException(error);
        }

        occasionOffer.Description = request.Description;
        occasionOffer.PricePerDay = request.PricePerDay;
        occasionOffer.GuestLimit = request.GuestLimit;
        occasionOffer.OfferStartDate = request.OfferStartDate;
        occasionOffer.OfferEndDate = request.OfferEndDate;
        occasionOffer.IsActive = request.IsActive;

        occasionOffer = await _occasionOfferDataAccess.Update(occasionOffer);

        // Update cache
        _cache.Set($"occasion_offer_{occasionOffer.Id}", occasionOffer,
            TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

        _logger.LogInformation("Event Offer {StayId} updated successfully", occasionOffer.Id);
        return occasionOffer.ToDto();
    }

    public async Task Delete(Guid id) {
        var occasionOffer = await _occasionOfferDataAccess.GetById(id);
        if (occasionOffer is null) {
            var errorMessage = "Event offer not found";
            _logger.LogError(errorMessage);
            throw new InvalidDataException(errorMessage);
        }

        await _occasionOfferDataAccess.Delete(occasionOffer);

        // Remove from cache
        _cache.Remove($"occasion_offer_{occasionOffer.Id}");

        _logger.LogInformation("Event Offer {Id} deleted successfully", occasionOffer.Id);
    }

    public async Task<IEnumerable<EventAvailabilityResponse>>
        SearchAvailability(EventSearchAvailabilityRequest request) {
        var result = await _eventOfferDataAccess.SearchAvailability(request);
        if (result == null || !result.Any()) {
            _logger.LogWarning("No event offers found for the given search criteria.");
            return new List<EventAvailabilityResponse>();
        }

        var availableOffers = result
            .Where(o => o.GuestLimit - o.GuestNumber >= request.NumberOfGuests)
            .Select(o => new EventAvailabilityResponse {
                EventOfferId = o.Id,
                EventName = o.Event.Name,
                PricePerDay = o.PricePerDay,
                AvailableCapacity = o.GuestLimit - o.GuestNumber,
                ImageSrc = _imageService.GetPicture(o.Event.Picture ?? " ").Result
            });

        return availableOffers;
    }
}