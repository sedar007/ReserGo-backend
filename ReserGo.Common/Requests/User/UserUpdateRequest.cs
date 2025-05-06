using ReserGo.Common.DTO;

namespace ReserGo.Common.Requests.User;

public class UserUpdateRequest {
    public required String FirstName { get; init; }
    public required String LastName { get; init; }
    public required String Email { get; init; }
    public required String Username { get; init; }
    public String? Bio { get; init; }
    public String? PhoneNumber { get; init; }
    public AddressDto? Address { get; init; }
}
