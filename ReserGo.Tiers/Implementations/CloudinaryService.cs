using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ReserGo.Tiers.Interfaces;
using ReserGo.Tiers.Models;

namespace ReserGo.Tiers.Implementations;

public class CloudinaryService : ICloudinaryService {
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<CloudinaryService> _logger;

    public CloudinaryService(ILogger<CloudinaryService> logger, CloudinaryModel cloudinary) {
        _logger = logger;
        _cloudinary = cloudinary.GetCloudinary();
    }

    public string GetPicture(string publicId) {
        _logger.LogInformation("Getting picture with publicId: {PublicId}", publicId);
        var url = _cloudinary.Api.UrlImgUp.Secure().BuildUrl(publicId);
        _logger.LogInformation("Generated URL: {Url}", url);
        return url;
    }

    public async Task<bool> DeleteImage(string publicId) {
        _logger.LogInformation("Deleting image with publicId: {PublicId}", publicId);

        var deleteParams = new DeletionParams(publicId) {
            Type = "upload",
            ResourceType = ResourceType.Image,
            Invalidate = true
        };

        var result = await _cloudinary.DestroyAsync(deleteParams);
        if (result.StatusCode != HttpStatusCode.OK) {
            _logger.LogWarning("Failed to delete image with publicId: {PublicId}", publicId);
            return false;
        }

        _logger.LogInformation("Image with publicId: {PublicId} deleted successfully", publicId);
        return true;
    }

    public async Task<string?> UploadImage(IFormFile file, Guid userId) {
        _logger.LogInformation("Uploading image with file name: {FileName}", file.FileName);


        var folderPath = $"admin/{userId}";
        var createFolderResult = await _cloudinary.CreateFolderAsync(folderPath);

        if (createFolderResult.StatusCode != HttpStatusCode.OK) {
            _logger.LogWarning("Failed to create folder: {Folder}", folderPath);
            return null;
        }

        var uploadParams = new ImageUploadParams {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            AssetFolder = folderPath,
            PublicId = $"{folderPath}/{Guid.NewGuid()}",
            Overwrite = true,
            Transformation = new Transformation().Quality("auto").FetchFormat("auto")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult == null) {
            _logger.LogWarning("Upload result is null");
            return null;
        }

        _logger.LogInformation("Upload successful. PublicId : {PublicId}", uploadResult.PublicId);
        return uploadResult.PublicId;
    }
}