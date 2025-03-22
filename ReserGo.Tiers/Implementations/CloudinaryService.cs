using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

using ReserGo.Tiers.Interfaces;
using ReserGo.Tiers.Models;

namespace ReserGo.Tiers.Implementations {
    public class CloudinaryService : ICloudinaryService {
        private readonly ILogger<CloudinaryService> _logger;
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(ILogger<CloudinaryService> logger, CloudinaryModel cloudinary) {
            _logger = logger;
            _cloudinary = cloudinary.GetCloudinary();
        }

        public async Task<string> GetPicture(string publicId) {
            _logger.LogInformation("Getting picture with publicId: {PublicId}", publicId);
            string url = _cloudinary.Api.UrlImgUp.BuildUrl(publicId);
            _logger.LogInformation("Generated URL: {Url}", url);
            return url;
        }

        public async Task<string?> UploadImage(IFormFile file, int userId) {
            _logger.LogInformation("Uploading image with file name: {FileName}", file.FileName);
            
            
            var folderPath = $"admin/{userId}";
            var createFolderResult = await _cloudinary.CreateFolderAsync(folderPath);

            if (createFolderResult.StatusCode != System.Net.HttpStatusCode.OK) {
                _logger.LogWarning("Failed to create folder: {Folder}", folderPath);
                return null;
            }
            
            ImageUploadParams uploadParams = new ImageUploadParams {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                AssetFolder = folderPath,
                PublicId = $"{folderPath}/{Guid.NewGuid()}",
                Overwrite = true,
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };
            
            ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult == null) {
                _logger.LogWarning("Upload result is null");
                return null;
            }

            _logger.LogInformation("Upload successful. PublicId : {PublicId}", uploadResult.PublicId);
            return uploadResult.PublicId;
        }
    }
}