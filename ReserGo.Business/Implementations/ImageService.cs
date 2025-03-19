using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using ReserGo.Business.Interfaces;
using ReserGo.Tiers.Interfaces;

namespace ReserGo.Business.Implementations {
    public class ImageService : IImageService {
        private readonly ILogger<ImageService> _logger;
        private readonly ICloudinaryService _cloudinary;

        public ImageService(ILogger<ImageService> logger, ICloudinaryService cloudinary) {
            _logger = logger;
            _cloudinary = cloudinary;
        }
        
        public async Task<string> GetPicture(string publicId) {
            _logger.LogInformation("Getting picture with publicId: {PublicId}", publicId);
            string url = await _cloudinary.GetPicture(publicId);
            _logger.LogInformation("Retrieved picture URL: {Url}", url);
            return url;
        }

        public async Task<string?> UploadImage(IFormFile file) {
            _logger.LogInformation("Uploading image with file name: {FileName}", file.FileName);
            string? url = await _cloudinary.UploadImage(file);
            if (url == null)
                _logger.LogWarning("Image upload failed for file: {FileName}", file.FileName);
            else 
                _logger.LogInformation("Image uploaded successfully. URL: {Url}", url);
            return url;
        }
    }
}
