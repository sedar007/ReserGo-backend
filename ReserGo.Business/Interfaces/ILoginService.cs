using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Business.Interfaces;

public interface ILoginService {
    //<AuthenticateResponse?> Login(LoginRequest request);
    Task<LoginDto?> Create(string password, User user);
}
