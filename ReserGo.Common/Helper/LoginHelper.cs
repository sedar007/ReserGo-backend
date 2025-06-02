using System.Globalization;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class LoginHelper {
    public static LoginDto ToDto(this Login login) {
        return new LoginDto {
            Id = login.Id,
            UserId = login.UserId,
            Username = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(login.Username.ToLower()),
            LastLogin = login.LastLogin,
            Password = login.Password
        };
    }
}