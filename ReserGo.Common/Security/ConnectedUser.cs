using ReserGo.Common.Enum;

namespace ReserGo.Common.Security;

public class ConnectedUser {
    public Guid UserId { get; init; }
    public UserRole Role { get; init; }
    public string? RoleString { get; set; }
    public string Username { get; set; } = null!;
}