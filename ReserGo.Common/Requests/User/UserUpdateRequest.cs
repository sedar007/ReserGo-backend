using ReserGo.Common.DTO;

namespace ReserGo.Common.Requests.User;

public class UserUpdateRequest {
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public string? Bio { get; init; }
    public string? PhoneNumber { get; init; }
    public AddressDto? Address { get; init; }
}