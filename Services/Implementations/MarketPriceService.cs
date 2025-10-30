public class MarketPriceService : IMarketPriceService
{
    private readonly IUnitOfWork _unitOfWork;

    public MarketPriceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MarketPriceStatisticsDto> GetPriceStatisticsAsync(int cropTypeId, int days = 30)
    {
        var startDate = DateTime.UtcNow.AddDays(-days);
        var prices = await _unitOfWork.MarketPrices
            .FindAsync(p => p.CropTypeId == cropTypeId && p.PriceDate >= startDate);

        var priceList = prices.OrderBy(p => p.PriceDate).ToList();

        if (!priceList.Any())
            return null;

        var cropType = await _unitOfWork.CropTypes.GetByIdAsync(cropTypeId);
        var latestPrice = priceList.Last();

        return new MarketPriceStatisticsDto
        {
            CropTypeId = cropTypeId,
            CropName = cropType?.NameEn,
            CurrentPrice = latestPrice.PricePerUnit,
            AveragePrice = priceList.Average(p => p.PricePerUnit),
            MinPrice = priceList.Min(p => p.PricePerUnit),
            MaxPrice = priceList.Max(p => p.PricePerUnit),
            Currency = latestPrice.Currency,
            Unit = latestPrice.Unit,
            PriceHistory = priceList.Select(p => new PriceHistoryPoint
            {
                Date = p.PriceDate,
                Price = p.PricePerUnit
            }).ToList()
        };
    }

    public async Task SyncMarketPricesAsync()
    {
        // TODO: Fetch from external market price API or scrape market websites
        // For now, generating sample data

        var cropTypes = await _unitOfWork.CropTypes.GetAllAsync();
        var random = new Random();

        foreach (var crop in cropTypes.Take(5)) // Sample for first 5 crops
        {
            var marketPrice = new MarketPrice
            {
                CropTypeId = crop.Id,
                MarketName = "Central Market",
                MarketLocation = "Cairo",
                PricePerUnit = 10 + random.Next(1, 50),
                Unit = "kg",
                Currency = "EGP",
                Quality = "Grade A",
                PriceDate = DateTime.UtcNow,
                Source = "Manual Entry"
            };

            await _unitOfWork.MarketPrices.AddAsync(marketPrice);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}