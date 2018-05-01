namespace CandleTimeSeriesAnalysis.Strategies
{
    public class Wallet : IWallet
    {
        public string StockId { get; }
        public double Share { get; private set; }
        public double Liquidity { get; private set; }

        public Wallet( string stockId, double share, double liquidity )
        {
            StockId = stockId;
            Share = share;
            Liquidity = liquidity;
        }

        public void Sell( double share, double price )
        {
            Share -= share;
            Liquidity += share * price;
        }
        public void Buy( double share, double price )
        {
            Share += share;
            Liquidity -= share * price;
        }
        public void Pay( double price )
        {
            Liquidity -= price;
        }
    }
}
