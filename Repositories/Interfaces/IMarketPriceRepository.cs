public interface IMarketPriceRepository : IRepository<MarketPrice>
{
    Task<IEnumerable<MarketPrice>> GetLatestPricesByCropAsync(int cropTypeId);
    Task<IEnumerable<MarketPrice>> GetPriceHistoryAsync(int cropTypeId, DateTime startDate, DateTime endDate);
    Task<decimal?> GetAveragePriceAsync(int cropTypeId, int days = 30);
}