namespace ReserGo.Common.Requests.User;

public class UserCreationRequest {
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}