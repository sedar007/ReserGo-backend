using Microsoft.Extensions.Logging;

using ReserGo.Business.Interfaces;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared.Interfaces;
using ReserGo.Common.Security;
using ReserGo.Tiers.Interfaces;
using ReserGo.Tiers.Responses;
using ReserGo.Common.Enum;

namespace ReserGo.Business.Implementations;
public class GoogleService : IGoogleService {
    
    private readonly ILogger<GoogleService> _logger;
    private readonly IUserDataAccess _userDataAccess;
    private readonly ISecurity _security;
    private readonly IGoogleAuthService _gooleAuthService;
    
    public GoogleService(ILogger<GoogleService> logger, IGoogleAuthService gooleAuthService , IUserDataAccess userDataAccess, ISecurity security) {
        _logger = logger;
        _gooleAuthService = gooleAuthService;
        _userDataAccess = userDataAccess;
        _security = security;
    }
    
    public async Task<AuthenticateResponse?> Auth(string? token) {
        _logger.LogInformation("Starting Google authentication process.");
        
        if(token == null) {
            _logger.LogWarning("Google Auth failed: token is null.");
            throw new ArgumentNullException("Google Auth failed");
        }

        GoogleAuthResponse? response = await _gooleAuthService.Create(token);
        if (response == null) {
            _logger.LogWarning("Google Auth failed: response is null.");
            throw new KeyNotFoundException("Google Auth failed");
        }
    
        _logger.LogInformation("Google Auth successful for email: {Email}", response.Email);

        User? user = await _userDataAccess.GetByEmail(response.Email);
        if (user == null) {
            _logger.LogInformation("User not found, creating new user with email: {Email}", response.Email);
            User newUser = new User {
                FirstName = response.GivenName,
                LastName = response.FamilyName,
                Email = response.Email,
                Username = response.Email,
                Role = UserRole.Admin
            };
            user = await _userDataAccess.Create(newUser);
            _logger.LogInformation("New user created with ID: {UserId}", user.Id);
        } else {
            _logger.LogInformation("User found with ID: {UserId}", user.Id);
        }

        string jwtToken = _security.GenerateJwtToken(user.Username, user.Id, user.Role);
        _logger.LogInformation("JWT token generated for user ID: {UserId}", user.Id);

        return new AuthenticateResponse(user, jwtToken, user.Role);
    }
    
}