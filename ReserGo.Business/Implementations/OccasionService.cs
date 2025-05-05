using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products.Occasion;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;
using ReserGo.Shared.Interfaces;

namespace ReserGo.Business.Implementations;

public class OccasionService : IOccasionService {

    private readonly IMemoryCache _cache;
    private readonly ILogger<UserService> _logger;
    private readonly ISecurity _security;
    private readonly IImageService _imageService;
    private readonly IOccasionDataAccess _occasionDataAccess;

    public OccasionService(IMemoryCache cache, ILogger<UserService> logger, IOccasionDataAccess occasionDataAccess, ISecurity security, IImageService imageService) {
        _cache = cache;
        _logger = logger;
        _security = security;
        _imageService = imageService;
        _occasionDataAccess = occasionDataAccess;
    }

    public async Task<OccasionDto> Create(OccasionCreationRequest request) {
        try {
            Occasion? occasion = await _occasionDataAccess.GetByStayId(request.StayId);
            if (occasion is not null) {
                string errorMessage = "This occasion already exists.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            string error = OccasionValidator.GetError(request);
            if (!string.IsNullOrEmpty(error)) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            ConnectedUser connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) throw new UnauthorizedAccessException("User not connected");

            Occasion newOccasion = new Occasion {
                Name = request.Name,
                Location = request.Location,
                Capacity = request.Capacity,
                StayId = request.StayId,
                Picture = request.Picture != null ? await _imageService.UploadImage(request.Picture, connectedUser.UserId) : null,
                UserId = connectedUser.UserId,
                LastUpdated = DateTime.UtcNow
            };

            newOccasion = await _occasionDataAccess.Create(newOccasion);

            _logger.LogInformation("Occasion { id } created", newOccasion.Id);

            // Set cache
            _cache.Set($"Occasion_GetById_{newOccasion.Id}", newOccasion.ToDto(), TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            _cache.Set($"Occasion_GetByStayId_{newOccasion.StayId}", newOccasion.ToDto(), TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            return newOccasion.ToDto();
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<OccasionDto?> GetById(int id) {
        try {
            string cacheKey = $"Occasion_GetById_{id}";

            if (_cache.TryGetValue(cacheKey, out OccasionDto? cachedOccasion)) {
                _logger.LogInformation("Returning cached occasion for ID: {Id}", id);
                return cachedOccasion;
            }

            Occasion? occasion = await _occasionDataAccess.GetById(id);
            if (occasion is null) {
                string errorMessage = "This occasion does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            _logger.LogInformation("Occasion { Id } retrieved successfully", occasion.Id);
            OccasionDto occasionDto = occasion.ToDto();
            _cache.Set(cacheKey, occasionDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            return occasionDto;

        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<OccasionDto?> GetByStayId(long stayId) {
        try {
            string cacheKey = $"Occasion_GetByStayId_{stayId}";

            if (_cache.TryGetValue(cacheKey, out OccasionDto? cachedOccasion)) {
                _logger.LogInformation("Returning cached occasion for StayId: {StayId}", stayId);
                return cachedOccasion;
            }

            Occasion? occasion = await _occasionDataAccess.GetByStayId(stayId);
            if (occasion is null) {
                string errorMessage = "This occasion does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            _logger.LogInformation("Occasion { StayId } retrieved successfully", occasion.StayId);
            OccasionDto occasionDto = occasion.ToDto();
            _cache.Set(cacheKey, occasionDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            return occasionDto;

        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<IEnumerable<OccasionDto>> GetOccasionsByUserId(int userId) {
        try {
            IEnumerable<Occasion> occasions = await _occasionDataAccess.GetOccasionsByUserId(userId);
            return occasions.Select(occasion => occasion.ToDto());
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<OccasionDto> Update(long stayId, OccasionUpdateRequest request) {
        try {
            Occasion? occasion = await _occasionDataAccess.GetByStayId(stayId);
            if (occasion is null) throw new Exception("Occasion not found");

            string error = OccasionValidator.GetError(request);
            if (!string.IsNullOrEmpty(error)) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            occasion.Name = request.Name;
            occasion.Location = request.Location;
            occasion.Capacity = request.Capacity;
            occasion.LastUpdated = DateTime.UtcNow;

            if (request.Picture != null) {
                string? oldPublicId = occasion.Picture;

                string? publicId = await _imageService.UploadImage(request.Picture, occasion.UserId);
                if (string.IsNullOrEmpty(publicId)) {
                    _logger.LogWarning("Image upload failed for file: {FileName}", request.Picture.FileName);
                    throw new InvalidDataException("Image upload failed.");
                }

                if (oldPublicId is not null) {
                    bool deleteResult = await _imageService.DeleteImage(oldPublicId);
                    if (!deleteResult) {
                        _logger.LogWarning("Failed to delete old image with publicId: {PublicId}", oldPublicId);
                    }
                }

                occasion.Picture = publicId;
            }

            _logger.LogInformation("Occasion { stayId } updated successfully", occasion.StayId);
            await _occasionDataAccess.Update(occasion);

            // Invalidate cache
            RemoveCache(occasion.Id, occasion.StayId);

            return occasion.ToDto();

        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task Delete(int id) {
        try {
            Occasion? occasion = await _occasionDataAccess.GetById(id);
            if (occasion is null) {
                string errorMessage = "Occasion not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }
            string? oldPublicId = occasion.Picture;
            await _occasionDataAccess.Delete(occasion);
            if (oldPublicId is not null) {
                bool deleteResult = await _imageService.DeleteImage(oldPublicId);
                if (!deleteResult) {
                    _logger.LogWarning("Failed to delete image with publicId: {PublicId}", oldPublicId);
                }
            }

            // Invalidate cache
            RemoveCache(occasion.Id, occasion.StayId);

            _logger.LogInformation("Occasion { id } deleted successfully", occasion.Id);

        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    private void RemoveCache(int id, long stayId) {
        _cache.Remove($"Occasion_GetById_{id}");
        _cache.Remove($"Occasion_GetByStayId_{stayId}");
    }
}
