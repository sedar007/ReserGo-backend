using ReserGo.Common.Enum;
namespace ReserGo.Common.Response;

public class LoginResponse {
    public String Message { get; init; } = String.Empty;
    public int Id { get; init; }
    public UserRole Role { get; init; }
    public String Username { get; init; } = String.Empty;
    public String RoleName { get; init; } = String.Empty;
}