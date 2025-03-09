using ReserGo.Common.Entity;
using ReserGo.Common.Enum;

namespace ReserGo.Common.Security {
    public class AuthenticateResponse {

        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public UserRole Role { get; set; }
        public string RoleName => Role.ToString();

        public AuthenticateResponse(User user, string token, UserRole role) {
            Id = user.Id;
            Username = user.Username;
            Token = token;
            Role = role;
        }
    }
}
