using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Enum;

namespace ReserGo.Common.Security;

public class AuthenticateResponse {
    public readonly Guid Id;
    public readonly string Username;
    public readonly string Token;
    public readonly UserRole Role;
    public string RoleName => Role.ToString();

    public AuthenticateResponse(UserDto user, string token, UserRole role) {
        Id = user.Id;
        Username = user.Username;
        Token = token;
        Role = role;
    }
}