using ReserGo.Common.Enum;

namespace ReserGo.Common.Response;

public class LoginResponse {
    public string Message { get; init; } = string.Empty;
    public Guid Id { get; init; }
    public UserRole Role { get; init; }
    public string Username { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
}