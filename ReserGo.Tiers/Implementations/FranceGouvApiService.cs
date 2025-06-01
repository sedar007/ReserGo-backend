using ReserGo.Tiers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ReserGo.Tiers.Implementations;

public class FranceGouvApiService : IFranceGouvApiService {
    private readonly ILogger<FranceGouvApiService> _logger;
    private readonly HttpClient _httpClient;
    private readonly string? _api;

    public FranceGouvApiService(IConfiguration configuration, ILogger<FranceGouvApiService> logger,
        HttpClient httpClient) {
        _logger = logger;
        _httpClient = httpClient;
        _api = configuration.GetSection("FranceGouvApi").Get<string>();
    }


    public async Task<IEnumerable<string?>> SearchAddresses(string query) {
        if (string.IsNullOrWhiteSpace(_api)) {
            _logger.LogError("FranceGouv API URL is not configured.");
            throw new InvalidOperationException("FranceGouv API URL is not configured.");
        }
        
        if (string.IsNullOrWhiteSpace(query) || query.Length < 3) {
            _logger.LogWarning("Invalid query: Query must be at least 3 characters long.");
            return new List<string>();
        }
        
        var encodedQuery = Uri.EscapeDataString(query);
        var requestUrl = _api.Replace("{encodedQuery}", encodedQuery);

        _logger.LogInformation("Sending request to FranceGouv API: {RequestUrl}", requestUrl);

        try {
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode) {
                _logger.LogError("Failed to fetch addresses. Status Code: {StatusCode}", response.StatusCode);
                throw new HttpRequestException($"Failed to fetch addresses: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Successfully received response from FranceGouv API.");

            var jsonResponse = System.Text.Json.JsonDocument.Parse(content);

            if (!jsonResponse.RootElement.TryGetProperty("features", out var features)) {
                _logger.LogWarning("Response does not contain 'features' property.");
                return new List<string>();
            }

            var properties = features.EnumerateArray()
                .Select(feature => feature.TryGetProperty("properties", out var props) &&
                                   props.TryGetProperty("label", out var label)
                    ? label.GetString()
                    : null)
                .Where(label => !string.IsNullOrEmpty(label))
                .ToList();

            _logger.LogInformation("Successfully parsed {Count} addresses.", properties.Count);
            return properties;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while searching for addresses.");
            return new List<string>();
        }
    }
}