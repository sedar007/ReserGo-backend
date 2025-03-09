using ReserGo.Common.Enum;

namespace ReserGo.Common.Security {

    public class CurrentUser {
        public int UserId { get; set; }
       public UserRole Role { get; set; } 
        public string Username { get; set; } = null!;
    }
}
