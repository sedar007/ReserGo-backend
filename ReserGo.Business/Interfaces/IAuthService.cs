using ReserGo.Common.Requests.Security;
using ReserGo.Common.Security;
namespace ReserGo.Business.Interfaces {
    public interface IAuthService{
        Task<AuthenticateResponse?> Login(LoginRequest request);
    }
}
