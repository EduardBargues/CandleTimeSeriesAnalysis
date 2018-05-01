namespace CandleTimeSeriesAnalysis.Strategies
{
    public interface IWallet
    {
        string StockId { get; }
        double Share { get; }
        double Liquidity { get; }
        void Sell( double share, double price );
        void Buy( double share, double price );
        void Pay( double price );
    }
}