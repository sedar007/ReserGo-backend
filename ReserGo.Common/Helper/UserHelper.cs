using Common.DTO;
using Common.Entity;

namespace Common.Helper;
public static class UserHelper {
    public static UserDTO ToDto(this User user) {
        return new UserDTO {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role,
            Bookings = user.Bookings
        };
    }
}