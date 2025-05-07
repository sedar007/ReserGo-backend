namespace ReserGo.Business.Interfaces;

public interface IFranceGouvService {
    Task<IEnumerable<string>?> SearchAddresses(string query);
}