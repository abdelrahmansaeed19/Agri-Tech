// <summary>
/// Syncs market prices daily
/// </summary>
public class MarketPriceSyncService : BackgroundService
{
    private readonly ILogger<MarketPriceSyncService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public MarketPriceSyncService(
        ILogger<MarketPriceSyncService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Market Price Sync Service started");

        // Wait 1 minute before first run
        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SyncMarketPricesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing market prices");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Market Price Sync Service stopped");
    }

    private async Task SyncMarketPricesAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var marketPriceService = scope.ServiceProvider.GetRequiredService<IMarketPriceService>();

        _logger.LogInformation("Starting market price sync");
        await marketPriceService.SyncMarketPricesAsync();
        _logger.LogInformation("Market price sync completed");
    }
}