namespace CandleTimeSeriesAnalysis.Strategies
{
    public interface IBroker
    {
        double GetBuyFee(string stockId, double share);
        double GetSellFee(string stockId, double share);
    }
}
