using System.ComponentModel.DataAnnotations;


namespace ReserGo.Common.Requests.Security;

public class LoginRequest {
    public required string Login { get; set; }

    [Required] public required string Password { get; set; }
}