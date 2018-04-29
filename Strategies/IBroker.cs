namespace CandleTimeSeriesAnalysis.Strategies
{
    public interface IBroker
    {
        decimal GetBuyFee(string stockId, decimal share);
        decimal GetSellFee(string stockId, decimal share);
    }
}
