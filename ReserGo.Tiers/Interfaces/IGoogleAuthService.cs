using ReserGo.Tiers.Responses;

namespace ReserGo.Tiers.Interfaces;

public interface IGoogleAuthService {
    Task<GoogleAuthResponse?> Create(string token);
}