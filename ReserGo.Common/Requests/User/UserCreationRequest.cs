namespace ReserGo.Common.Requests.User;

public class UserCreationRequest {
    public required String FirstName { get; init; }
    public required String LastName { get; init; }
    public required String Email { get; init; }
    public required String Username { get; init; }
    public required String Password { get; init; }
}