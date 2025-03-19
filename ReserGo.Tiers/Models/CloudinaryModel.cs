using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CloudinaryDotNet;

namespace ReserGo.Tiers.Models;

public class CloudinaryModel {
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<CloudinaryModel> _logger;

    public CloudinaryModel(IConfiguration configuration, ILogger<CloudinaryModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("Initializing CloudinaryModel");

        var cloudinarySettings = configuration.GetSection("Cloudinary").Get<AppSettingsCloudinary>();
        if (cloudinarySettings == null)
        {
            _logger.LogError("Cloudinary settings are not configured properly.");
            throw new ArgumentNullException(nameof(cloudinarySettings), "Cloudinary settings are missing.");
        }

        var account = new Account(cloudinarySettings.CloudName, cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret);
        _cloudinary = new Cloudinary(account);

        _logger.LogInformation("CloudinaryModel initialized successfully");
    }

    public Cloudinary GetCloudinary()
    {
        _logger.LogInformation("Retrieving Cloudinary instance");
        return _cloudinary;
    }
}

public class AppSettingsCloudinary {
    public string CloudName { get; init; } = null!;
    public string ApiKey { get; init; } = null!;
    public string ApiSecret { get; init; } = null!;
}