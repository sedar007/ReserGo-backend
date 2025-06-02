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

public class HotelOfferService : IHotelOfferService {
    private readonly IMemoryCache _cache;
    private readonly IHotelOfferDataAccess _hotelOfferDataAccess;
    private readonly IHotelService _hotelService;
    private readonly ILogger<HotelOfferService> _logger;
    private readonly ISecurity _security;

    public HotelOfferService(ILogger<HotelOfferService> logger, IHotelOfferDataAccess hotelOfferDataAccess,
        IHotelService hotelService, ISecurity security, IMemoryCache cache) {
        _logger = logger;
        _security = security;
        _hotelOfferDataAccess = hotelOfferDataAccess;
        _hotelService = hotelService;
        _cache = cache;
    }

    public async Task<HotelOfferDto> Create(HotelOfferCreationRequest request) {
       
            var error = HotelOfferValidator.GetError(request);
            if (!string.IsNullOrEmpty(error)) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) throw new UnauthorizedAccessException("User not connected");

            var hotel = await _hotelService.GetById(request.HotelId);
            if (hotel == null) {
                var errorMessage = "Hotel not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var newHotelOffer = new HotelOffer {
                OfferTitle = request.OfferTitle,
                Description = request.Description,
                PricePerNight = request.PricePerNight,
                NumberOfGuests = request.NumberOfGuests,
                NumberOfRooms = request.NumberOfRooms,
                OfferStartDate = request.OfferStartDate,
                OfferEndDate = request.OfferEndDate,
                IsActive = request.IsActive,
                HotelId = hotel.Id,
                UserId = connectedUser.UserId
            };

            newHotelOffer = await _hotelOfferDataAccess.Create(newHotelOffer);

            // Cache the created hotel offer
            var cacheKey = string.Format(Consts.CacheKeyHotelOffers, connectedUser.UserId);
            if (_cache.TryGetValue(cacheKey, out List<HotelOfferDto>? cachedHotelOffers)) {
                cachedHotelOffers ??= new List<HotelOfferDto>();
                cachedHotelOffers.Add(newHotelOffer.ToDto());
                _cache.Set(cacheKey, cachedHotelOffers, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            }
            else {
                var newCacheList = new List<HotelOfferDto> { newHotelOffer.ToDto() };
                _cache.Set(cacheKey, newCacheList, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            }


            _logger.LogInformation("Hotel Offer {Id} created", newHotelOffer.Id);
            return newHotelOffer.ToDto();
        
    }

    public async Task<HotelOfferDto?> GetById(Guid id) {
       
            if (_cache.TryGetValue($"hotelOffer_{id}", out HotelOffer? cachedHotelOffer))
                if (cachedHotelOffer != null)
                    return cachedHotelOffer.ToDto();

            var hotelOffer = await _hotelOfferDataAccess.GetById(id);
            if (hotelOffer is null) {
                var errorMessage = "This hotel offer does not exist.";
                _logger.LogError(errorMessage);
                return null;
            }

            _cache.Set($"hotelOffer_{id}", hotelOffer, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            _logger.LogInformation("Hotel Offer {Id} retrieved successfully", hotelOffer.Id);
            return hotelOffer.ToDto();
        
    }

    public async Task<IEnumerable<HotelOfferDto>> GetHotelsByUserId(Guid userId) {
       
            var cacheKey = string.Format(Consts.CacheKeyHotelOffers, userId);

            if (_cache.TryGetValue(cacheKey, out List<HotelOfferDto>? cachedHotelOffers))
                if (cachedHotelOffers != null)
                    return cachedHotelOffers;

            var hotelOffers = await _hotelOfferDataAccess.GetHotelsOfferByUserId(userId);
            var hotelOfferDtos = hotelOffers.Select(hotelOffer => hotelOffer.ToDto());

            _cache.Set(cacheKey, hotelOfferDtos, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            return hotelOfferDtos;
        
    }

    public async Task<HotelOfferDto> Update(Guid id, HotelOfferUpdateRequest request) {
       
            var hotelOffer = await _hotelOfferDataAccess.GetById(id);
            if (hotelOffer is null) throw new InvalidDataException("Hotel offer not found");

            var error = HotelOfferValidator.GetError(request);
            if (!string.IsNullOrEmpty(error)) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            hotelOffer.OfferTitle = request.OfferTitle;
            hotelOffer.Description = request.Description;
            hotelOffer.PricePerNight = request.PricePerNight;
            hotelOffer.NumberOfGuests = request.NumberOfGuests;
            hotelOffer.NumberOfRooms = request.NumberOfRooms;
            hotelOffer.OfferStartDate = request.OfferStartDate;
            hotelOffer.OfferEndDate = request.OfferEndDate;
            hotelOffer.IsActive = request.IsActive;

            hotelOffer = await _hotelOfferDataAccess.Update(hotelOffer);

            // Update cache
            _cache.Set($"hotel_offer_{hotelOffer.Id}", hotelOffer, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            _logger.LogInformation("Hotel Offer {StayId} updated successfully", hotelOffer.Id);
            return hotelOffer.ToDto();
        
    }

    public async Task Delete(Guid id) {
        
            var hotelOffer = await _hotelOfferDataAccess.GetById(id);
            if (hotelOffer is null) {
                var errorMessage = "Hotel offer not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            await _hotelOfferDataAccess.Delete(hotelOffer);

            // Remove from cache
            _cache.Remove($"hotel_offer_{hotelOffer.Id}");

            _logger.LogInformation("Hotel Offer {Id} deleted successfully", hotelOffer.Id);
       
    }
}