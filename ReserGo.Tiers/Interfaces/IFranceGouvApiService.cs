namespace ReserGo.Tiers.Interfaces;

public interface IFranceGouvApiService {
    Task<IEnumerable<string?>> SearchAddresses(string query);
}