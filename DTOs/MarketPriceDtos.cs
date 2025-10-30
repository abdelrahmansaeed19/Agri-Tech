public class MarketPriceDto
{
    public int Id { get; set; }
    public int CropTypeId { get; set; }
    public string CropName { get; set; }
    public string MarketName { get; set; }
    public string MarketLocation { get; set; }
    public decimal PricePerUnit { get; set; }
    public string Unit { get; set; }
    public string Currency { get; set; }
    public string Quality { get; set; }
    public DateTime PriceDate { get; set; }
    public decimal? PriceChange { get; set; } // Percentage change from previous
    public string PriceTrend { get; set; } // Up, Down, Stable
}

public class MarketPriceStatisticsDto
{
    public int CropTypeId { get; set; }
    public string CropName { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal? AveragePrice { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string Currency { get; set; }
    public string Unit { get; set; }
    public List<PriceHistoryPoint> PriceHistory { get; set; }
}

public class PriceHistoryPoint
{
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
}