using ReserGo.Tiers.Interfaces;
using ReserGo.Tiers.Responses;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;

namespace ReserGo.Tiers.Implementations;

public class GoogleAuthService : IGoogleAuthService {
    private readonly ILogger<GoogleAuthService> _logger;

    public GoogleAuthService(ILogger<GoogleAuthService> logger) {
        _logger = logger;
    }

    public async Task<GoogleAuthResponse?> Create(string token) {
        _logger.LogInformation("Starting Google token validation.");

        try {
            // Validate the Google token
            var payload = await GoogleJsonWebSignature.ValidateAsync(token);
            _logger.LogInformation("Google token validated successfully.");

            // Retrieve user info
            var userInfo = new GoogleAuthResponse {
                Email = payload.Email,
                FamilyName = payload.FamilyName,
                GivenName = payload.GivenName
            };

            _logger.LogInformation("User info retrieved: {Email}", userInfo.Email);
            return userInfo;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error occurred during Google token validation.");
            return null;
        }
    }
}