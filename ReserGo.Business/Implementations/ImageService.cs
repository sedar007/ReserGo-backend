using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Tiers.Interfaces;

namespace ReserGo.Business.Implementations;

public class ImageService : IImageService {
    private readonly ILogger<ImageService> _logger;
    private readonly ICloudinaryService _cloudinary;
    private readonly IMemoryCache _cache;

    public ImageService(ILogger<ImageService> logger, ICloudinaryService cloudinary, IMemoryCache cache) {
        _logger = logger;
        _cloudinary = cloudinary;
        _cache = cache;
    }

    public async Task<string> GetPicture(string publicId) {
        _logger.LogInformation("Getting picture with publicId: {PublicId}", publicId);
        var cacheKey = $"GetPicture_{publicId}";
        if (_cache.TryGetValue(cacheKey, out string cachedUrl)) {
            _logger.LogInformation("Returning cached URL for publicId: {PublicId}", publicId);
            return cachedUrl;
        }

        var url = await _cloudinary.GetPicture(publicId);
        _logger.LogInformation("Retrieved picture URL: {Url}", url);

        _cache.Set(cacheKey, url, TimeSpan.FromMinutes(30));

        return url;
    }

    public async Task<string?> UploadImage(IFormFile file, int userId) {
        _logger.LogInformation("Uploading image with file name: {FileName}", file.FileName);
        var publicId = await _cloudinary.UploadImage(file, userId);
        if (publicId == null)
            _logger.LogWarning("Image upload failed for file: {FileName}", file.FileName);
        else
            _logger.LogInformation("Image uploaded successfully. URL: {Url}", publicId);
        return publicId;
    }

    public async Task<bool> DeleteImage(string publicId) {
        _logger.LogInformation("Deleting image with publicId: {PublicId}", publicId);
        var result = await _cloudinary.DeleteImage(publicId);
        if (!result)
            _logger.LogWarning("Failed to delete image with publicId: {PublicId}", publicId);
        else
            _logger.LogInformation("Image with publicId: {PublicId} deleted successfully", publicId);
        return result;
    }
}