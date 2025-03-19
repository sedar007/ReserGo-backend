using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Requests.Security;
using ReserGo.Common.Security;

namespace ReserGo.Business.Interfaces;
public interface ILoginService {
    Task<LoginDto?> Create(string password, User user);
    Task<AuthenticateResponse?> Login(LoginRequest request);
}
