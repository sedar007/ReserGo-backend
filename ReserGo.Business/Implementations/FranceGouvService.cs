using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Tiers.Interfaces;

namespace ReserGo.Business.Implementations;

public class FranceGouvService : IFranceGouvService {
    private readonly IFranceGouvApiService _franceGouvApiService;
    private readonly ILogger<FranceGouvService> _logger;

    public FranceGouvService(ILogger<FranceGouvService> logger, IFranceGouvApiService franceGouvApiService) {
        _logger = logger;
        _franceGouvApiService = franceGouvApiService;
    }

    public async Task<IEnumerable<string>?> SearchAddresses(string query) {
        _logger.LogInformation("Starting address search with query: {Query}", query);

        if (string.IsNullOrWhiteSpace(query)) {
            _logger.LogWarning("Search query is null or empty.");
            return Enumerable.Empty<string>();
        }

        var addresses = await _franceGouvApiService.SearchAddresses(query);
        if (addresses == null) {
            _logger.LogWarning("No addresses found for the query: {Query}", query);
            return Enumerable.Empty<string>();
        }

        var addressList = addresses?.ToList();

        if (addressList == null || !addressList.Any()) {
            _logger.LogWarning("No addresses found for the query: {Query}", query);
            return Enumerable.Empty<string>();
        }

        _logger.LogInformation("Found {Count} addresses for the query: {Query}", addressList.Count, query);
        return addressList;
    }
}