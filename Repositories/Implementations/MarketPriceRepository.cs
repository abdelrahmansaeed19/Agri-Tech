using AgriculturalTech.API.Data;
using Microsoft.EntityFrameworkCore;

public class MarketPriceRepository : Repository<MarketPrice>, IMarketPriceRepository
{
    public MarketPriceRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<MarketPrice>> GetLatestPricesByCropAsync(int cropTypeId)
    {
        var latestDate = await _dbSet
            .Where(p => p.CropTypeId == cropTypeId)
            .MaxAsync(p => (DateTime?)p.PriceDate);

        if (!latestDate.HasValue)
            return new List<MarketPrice>();

        return await _dbSet
            .Include(p => p.CropType)
            .Where(p => p.CropTypeId == cropTypeId && p.PriceDate.Date == latestDate.Value.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<MarketPrice>> GetPriceHistoryAsync(int cropTypeId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(p => p.CropType)
            .Where(p => p.CropTypeId == cropTypeId
                && p.PriceDate >= startDate
                && p.PriceDate <= endDate)
            .OrderBy(p => p.PriceDate)
            .ToListAsync();
    }

    public async Task<decimal?> GetAveragePriceAsync(int cropTypeId, int days = 30)
    {
        var startDate = DateTime.UtcNow.AddDays(-days);

        var prices = await _dbSet
            .Where(p => p.CropTypeId == cropTypeId && p.PriceDate >= startDate)
            .ToListAsync();

        return prices.Any() ? prices.Average(p => p.PricePerUnit) : null;
    }
}