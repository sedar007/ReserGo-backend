
using ReserGo.Common.Enum;
using ReserGo.Common.Security;
using Microsoft.AspNetCore.Http;

namespace ReserGo.Shared.Interfaces {
    public interface ISecurity {
         string GenerateJwtToken(string username, int userId, UserRole userRole);

         string HashPassword(string providedPassword);

         bool VerifyPassword(string hashedPassword, string providedPassword);

        CurrentUser? GetCurrentUser();
        CookieOptions GetCookiesOptions();

    }
    
}
