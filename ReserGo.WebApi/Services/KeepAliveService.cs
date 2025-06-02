namespace ReserGo.WebAPI.Services;

public class KeepAliveService : IHostedService, IDisposable {
    private readonly HttpClient _httpClient;
    private readonly ILogger<KeepAliveService> _logger;
    private readonly string _url;
    private Timer _timer;


    public KeepAliveService(IConfiguration configuration, ILogger<KeepAliveService> logger) {
        _httpClient = new HttpClient();
        _logger = logger;
        _url = configuration.GetSection("urlInstance")?.Get<string>() ?? string.Empty;
        _logger.LogInformation("Keep-alive URL: {Url}", _url);
    }

    public void Dispose() {
        _timer?.Dispose();
        _httpClient?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Keep-alive service started");
        _timer = new Timer(SendKeepAliveRequest, null, TimeSpan.Zero, TimeSpan.FromMinutes(14));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private async void SendKeepAliveRequest(object state) {
        try {
            var response = await _httpClient.GetAsync(_url);
            if (response.IsSuccessStatusCode)
                _logger.LogInformation("Keep-alive successful at {Date}", DateTime.Now);
            else
                _logger.LogWarning("Failed keep-alive at {Date}: {Response}", DateTime.Now, response.StatusCode);
        }
        catch (Exception ex) {
            _logger.LogError("Exception in keep-alive: {Message}", ex.Message);
        }
    }
}