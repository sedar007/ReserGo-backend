using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products;
using ReserGo.Common.Requests.Products.Occasion;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared.Interfaces;
using ReserGo.Shared;

namespace ReserGo.Business.Implementations;

public class OccasionOfferService : IOccasionOfferService {
    private readonly ILogger<UserService> _logger;
    private readonly ISecurity _security;
    private readonly IImageService _imageService;
    private readonly IOccasionService _occasionService;
    private readonly IOccasionOfferDataAccess _occasionOfferDataAccess;
    private readonly IMemoryCache _cache;

    public OccasionOfferService(ILogger<UserService> logger, IOccasionOfferDataAccess occasionOfferDataAccess,
        IOccasionService occasionService, ISecurity security, IImageService imageService, IMemoryCache cache) {
        _logger = logger;
        _security = security;
        _imageService = imageService;
        _occasionOfferDataAccess = occasionOfferDataAccess;
        _occasionService = occasionService;
        _cache = cache;
    }

    public async Task<OccasionOfferDto> Create(OccasionOfferCreationRequest request) {
        try {
            var error = OccasionOfferValidator.GetError(request);
            if (string.IsNullOrEmpty(error) == false) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) throw new UnauthorizedAccessException("User not connected");

            var occasion = await _occasionService.GetById(request.OccasionId);
            if (occasion == null) {
                var errorMessage = "Occasion not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var newOccasionOffer = new OccasionOffer {
                OfferTitle = request.OfferTitle,
                Description = request.Description,
                Price = request.Price,
                NumberOfGuests = request.NumberOfGuests,
                OfferStartDate = request.OfferStartDate,
                OfferEndDate = request.OfferEndDate,
                IsActive = request.IsActive,
                OccasionId = occasion.Id,
                UserId = connectedUser.UserId
            };

            newOccasionOffer = await _occasionOfferDataAccess.Create(newOccasionOffer);

            // Cache the created occasion offer
            _cache.Set($"newOccasionOffer_{newOccasionOffer.Id}", newOccasionOffer,
                TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            _logger.LogInformation("Occasion Offer { id } created", newOccasionOffer.Id);
            return newOccasionOffer.ToDto();
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<OccasionOfferDto?> GetById(Guid id) {
        try {
            if (_cache.TryGetValue($"occasionOffer_{id}", out OccasionOffer cachedOccasionOffer))
                return cachedOccasionOffer.ToDto();

            var occasionOffer = await _occasionOfferDataAccess.GetById(id);
            if (occasionOffer is null) {
                var errorMessage = "This occasion offer does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            _cache.Set($"occasionOffer_{id}", occasionOffer, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            _logger.LogInformation("Occasion Offer { id } retrieved successfully", occasionOffer.Id);
            return occasionOffer.ToDto();
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<IEnumerable<OccasionOfferDto>> GetOccasionsByUserId(Guid userId) {
        try {
            var cacheKey = $"occasionOffers_user_{userId}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<OccasionOfferDto> cachedOccasionOffers))
                return cachedOccasionOffers;

            var occasionOffers = await _occasionOfferDataAccess.GetOccasionsOfferByUserId(userId);
            IEnumerable<OccasionOfferDto> occasionOfferDtos =
                occasionOffers.Select(occasionOffer => occasionOffer.ToDto());

            _cache.Set(cacheKey, occasionOfferDtos, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            return occasionOfferDtos;
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<OccasionOfferDto> Update(Guid id, OccasionOfferUpdateRequest request) {
        try {
            var occasionOffer = await _occasionOfferDataAccess.GetById(id);
            if (occasionOffer is null) throw new Exception("Occasion offer not found");

            var error = OccasionOfferValidator.GetError(request);
            if (string.IsNullOrEmpty(error) == false) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            occasionOffer.OfferTitle = request.OfferTitle;
            occasionOffer.Description = request.Description;
            occasionOffer.Price = request.Price;
            occasionOffer.NumberOfGuests = request.NumberOfGuests;
            occasionOffer.OfferStartDate = request.OfferStartDate;
            occasionOffer.OfferEndDate = request.OfferEndDate;
            occasionOffer.IsActive = request.IsActive;

            occasionOffer = await _occasionOfferDataAccess.Update(occasionOffer);

            // Update cache
            _cache.Set($"occasion_offer_{occasionOffer.Id}", occasionOffer,
                TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            _logger.LogInformation("Occasion Offer { stayId } updated successfully", occasionOffer.Id);
            return occasionOffer.ToDto();
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task Delete(Guid id) {
        try {
            var occasionOffer = await _occasionOfferDataAccess.GetById(id);
            if (occasionOffer is null) {
                var errorMessage = "Occasion offer not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            await _occasionOfferDataAccess.Delete(occasionOffer);

            // Remove from cache
            _cache.Remove($"occasion_offer_{occasionOffer.Id}");

            _logger.LogInformation("Occasion Offer { id } deleted successfully", occasionOffer.Id);
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}