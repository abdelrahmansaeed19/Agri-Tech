public interface IMarketPriceService
{
    Task<MarketPriceStatisticsDto> GetPriceStatisticsAsync(int cropTypeId, int days = 30);
    Task SyncMarketPricesAsync();
}