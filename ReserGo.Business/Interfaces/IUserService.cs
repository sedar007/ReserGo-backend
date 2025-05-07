using Microsoft.AspNetCore.Http;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.User;

namespace ReserGo.Business.Interfaces;

public interface IUserService {
    Task Delete(Guid id);
    Task<UserDto?> GetById(Guid id);
    Task<UserDto> Create(UserCreationRequest request);
    Task<UserDto> UpdateUser(Guid id, UserUpdateRequest request);
    Task<UserDto?> GetByEmail(string email);
    Task<UserDto?> GetByUsername(string username);

    Task<string> GetProfilePicture(Guid userId);
    Task<string> UpdateProfilePicture(Guid userId, IFormFile file);
}