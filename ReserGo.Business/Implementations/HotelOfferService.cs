using Microsoft.Extensions.Caching.Memory;
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
using ReserGo.Shared;

namespace ReserGo.Business.Implementations;

public class HotelOfferService : IHotelOfferService {
    private readonly ILogger<UserService> _logger;
    private readonly ISecurity _security;
    private readonly IImageService _imageService;
    private readonly IHotelService _hotelService;
    private readonly IHotelOfferDataAccess _hotelOfferDataAccess;
    private readonly IMemoryCache _cache;

    public HotelOfferService(ILogger<UserService> logger, IHotelOfferDataAccess hotelOfferDataAccess,
        IHotelService hotelService, ISecurity security, IImageService imageService, IMemoryCache cache) {
        _logger = logger;
        _security = security;
        _imageService = imageService;
        _hotelOfferDataAccess = hotelOfferDataAccess;
        _hotelService = hotelService;
        _cache = cache;
    }

    public async Task<HotelOfferDto> Create(HotelOfferCreationRequest request) {
        try {
            var error = HotelOfferValidator.GetError(request);
            if (string.IsNullOrEmpty(error) == false) {
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
            _cache.Set($"newHotelOffer_{newHotelOffer.Id}", newHotelOffer,
                TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            _logger.LogInformation("Hotel Offer { id } created", newHotelOffer.Id);
            return newHotelOffer.ToDto();
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<HotelOfferDto?> GetById(Guid id) {
        try {
            if (_cache.TryGetValue($"hotelOffer_{id}", out HotelOffer cachedHotelOffer))
                return cachedHotelOffer.ToDto();

            var hotelOffer = await _hotelOfferDataAccess.GetById(id);
            if (hotelOffer is null) {
                var errorMessage = "This hotel offer does not exist.";
                _logger.LogError(errorMessage);
                return null;
            }

            _cache.Set($"hotelOffer_{id}", hotelOffer, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            _logger.LogInformation("Hotel Offer { id } retrieved successfully", hotelOffer.Id);
            return hotelOffer.ToDto();
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<IEnumerable<HotelOfferDto>> GetHotelsByUserId(Guid userId) {
        try {
            var cacheKey = $"hotelOffers_user_{userId}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<HotelOfferDto> cachedHotelOffers))
                return cachedHotelOffers;

            var hotelOffers = await _hotelOfferDataAccess.GetHotelsOfferByUserId(userId);
            IEnumerable<HotelOfferDto> hotelOfferDtos = hotelOffers.Select(hotelOffer => hotelOffer.ToDto());

            _cache.Set(cacheKey, hotelOfferDtos, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            return hotelOfferDtos;
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<HotelOfferDto> Update(Guid id, HotelOfferUpdateRequest request) {
        try {
            var hotelOffer = await _hotelOfferDataAccess.GetById(id);
            if (hotelOffer is null) throw new Exception("Hotel offer not found");

            var error = HotelOfferValidator.GetError(request);
            if (string.IsNullOrEmpty(error) == false) {
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

            _logger.LogInformation("Hotel Offer { stayId } updated successfully", hotelOffer.Id);
            return hotelOffer.ToDto();
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task Delete(Guid id) {
        try {
            var hotelOffer = await _hotelOfferDataAccess.GetById(id);
            if (hotelOffer is null) {
                var errorMessage = "Hotel offer not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            await _hotelOfferDataAccess.Delete(hotelOffer);

            // Remove from cache
            _cache.Remove($"hotel_offer_{hotelOffer.Id}");

            _logger.LogInformation("Hotel Offer { id } deleted successfully", hotelOffer.Id);
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}