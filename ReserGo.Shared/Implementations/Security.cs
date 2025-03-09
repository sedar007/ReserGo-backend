using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using ReserGo.Common.Security;
using ReserGo.Shared.Interfaces;
using ReserGo.Common.Enum;

namespace ReserGo.Shared.Implementations {
    public class Security : ISecurity {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<Security> _logger;
        
        public Security(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<Security> logger) {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public string GenerateJwtToken(string username, int userId, UserRole userRole) {
            String keyJwt = _configuration.GetSection("Key")?.Get<string>() ?? string.Empty;
            String issuerJwt = _configuration.GetSection("Issuer")?.Value ?? string.Empty;
            String audienceJwt = _configuration.GetSection("Audience")?.Get<string>() ?? string.Empty;
            int expireMinutesJwt = _configuration.GetSection("ExpireMinutes")?.Get<int>() ?? 0;

            if (string.IsNullOrEmpty(keyJwt))
                keyJwt = GetEnvironmentVariable("Key");
            if (string.IsNullOrEmpty(issuerJwt))
                issuerJwt = GetEnvironmentVariable("Issuer");
            if (string.IsNullOrEmpty(audienceJwt))
                audienceJwt = GetEnvironmentVariable("Audience");
            if (expireMinutesJwt == 0)
                expireMinutesJwt = int.Parse(GetEnvironmentVariable("ExpireMinutes"));

            if (string.IsNullOrEmpty(keyJwt) || string.IsNullOrEmpty(issuerJwt) || string.IsNullOrEmpty(audienceJwt) || expireMinutesJwt == 0)
                throw new InvalidDataException("Jwt settings are not configured properly");

            var jwtSettings = new JwtSettings {
                Key = keyJwt,
                Issuer = issuerJwt,
                Audience = audienceJwt,
                ExpireMinutes = expireMinutesJwt
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = GetClaims(username, userId, userRole);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.ExpireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public CurrentUser? GetCurrentUser() {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = user?.FindFirst("UserRole")?.Value;
            var username = user?.Identity?.Name;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole)) {
                _logger.LogWarning("User not found");
                return null;
            }
            return new CurrentUser {
                UserId = int.Parse(userId),
                Username = username,
                Role = userRole == UserRole.Admin.ToString() ? UserRole.Admin : UserRole.Client
            };
        }

        public string HashPassword(string providedPassword) {
            if (string.IsNullOrEmpty(providedPassword)) return "";
            PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
            return hasher.HashPassword(new IdentityUser(), providedPassword);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword) {
            PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
            return hasher.VerifyHashedPassword(new IdentityUser(),
                hashedPassword, providedPassword) == PasswordVerificationResult.Success;
        }

        private static string GetEnvironmentVariable(string key) {
            return Environment.GetEnvironmentVariable(key) ?? "";
        }

        private static Claim[] GetClaims(string username, int userId, UserRole userRole) {
            return new[] {
                    new Claim("UserRole", userRole.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
        }
    }
}
