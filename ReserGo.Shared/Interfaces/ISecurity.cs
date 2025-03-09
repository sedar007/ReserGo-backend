
using ReserGo.Common.Enum;
using ReserGo.Common.Security;

namespace ReserGo.Shared.Interfaces {
    public interface ISecurity {
         string GenerateJwtToken(string username, int userId, UserRole userRole);

         string HashPassword(string providedPassword);

         bool VerifyPassword(string hashedPassword, string providedPassword);

        CurrentUser? GetCurrentUser();

    }
    
}
