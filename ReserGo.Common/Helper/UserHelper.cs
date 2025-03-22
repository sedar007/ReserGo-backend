using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;
public static class UserHelper {
    public static UserDto ToDto(this User user) {
        return new UserDto {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Bio = user.Bio,
            Role = user.Role,
            ProfilePicture =  user.ProfilePicture,
            Address = user.Address?.ToDto(),
            RoleString = user.Role.ToString(),
        };
    }
}