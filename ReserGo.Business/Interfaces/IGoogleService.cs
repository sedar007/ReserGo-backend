using ReserGo.Common.Security;

namespace ReserGo.Business.Interfaces;

public interface IGoogleService {
    Task<AuthenticateResponse?> Auth(string? token);
}