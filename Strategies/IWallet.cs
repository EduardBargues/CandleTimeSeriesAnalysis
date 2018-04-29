namespace CandleTimeSeriesAnalysis.Strategies
{
    public interface IWallet
    {
        string StockId { get; }
        decimal Share { get; }
        decimal Liquidity { get; }
        void Sell( decimal share, decimal price );
        void Buy( decimal share, decimal price );
        void Pay( decimal price );
    }
}