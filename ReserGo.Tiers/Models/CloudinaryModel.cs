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
        string? cloudinaryCloudName = configuration.GetSection("CloudinaryCloudName")?.Get<string>() ;
        string? cloudinaryApiKey = configuration.GetSection("CloudinaryApiKey")?.Get<string>();
        string? cloudinaryApiSecret = configuration.GetSection("CloudinaryApiSecret")?.Get<string>();
        if(cloudinaryCloudName == null || cloudinaryApiKey == null || cloudinaryApiSecret == null)
        {
            _logger.LogError("Cloudinary settings are not configured properly.");
            throw new ArgumentNullException(nameof(cloudinaryCloudName), "Cloudinary settings are missing.");
        }
       

        var account = new Account(cloudinaryCloudName, cloudinaryApiKey, cloudinaryApiSecret);
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
    public string CloudinaryCloudName { get; init; } = null!;
    public string CloudinaryApiKey { get; init; } = null!;
    public string CloudinaryApiSecret { get; init; } = null!;
}