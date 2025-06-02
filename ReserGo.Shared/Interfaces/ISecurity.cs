using Microsoft.AspNetCore.Http;
using ReserGo.Common.Enum;
using ReserGo.Common.Security;

namespace ReserGo.Shared.Interfaces;

public interface ISecurity {
    string GenerateJwtToken(string username, Guid userId, UserRole userRole);

    string HashPassword(string providedPassword);

    bool VerifyPassword(string hashedPassword, string providedPassword);

    ConnectedUser? GetCurrentUser();
    CookieOptions GetCookiesOptions();
}