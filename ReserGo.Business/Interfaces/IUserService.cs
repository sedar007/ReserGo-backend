using ReserGo.Common.DTO;
using ReserGo.Common.Requests.User;

namespace ReserGo.Business.Interfaces;

public interface IUserService {
    Task Delete(int id);
    Task<UserDto?> GetById(int id);
    Task<UserDto> Create(UserCreationRequest request);
    Task<UserDto> UpdateUser(int id, UserUpdateRequest request);
    Task<UserDto?> GetByEmail(string email);
    Task<UserDto?> GetByUsername(string username);
    
}