using ReserGo.Common.DTO;
using ReserGo.Common.Enum;

namespace ReserGo.Common.Security;

public class AuthenticateResponse {
    public readonly Guid Id;
    public readonly UserRole Role;
    public readonly string Token;
    public readonly string Username;

    public AuthenticateResponse(UserDto user, string token, UserRole role) {
        Id = user.Id;
        Username = user.Username;
        Token = token;
        Role = role;
    }

    public string RoleName => Role.ToString();
}