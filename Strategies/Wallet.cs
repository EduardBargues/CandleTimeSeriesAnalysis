namespace CandleTimeSeriesAnalysis.Strategies
{
    public class Wallet : IWallet
    {
        public string StockId { get; }
        public decimal Share { get; private set; }
        public decimal Liquidity { get; private set; }

        public Wallet( string stockId, decimal share, decimal liquidity )
        {
            StockId = stockId;
            Share = share;
            Liquidity = liquidity;
        }

        public void Sell( decimal share, decimal price )
        {
            Share -= share;
            Liquidity += share * price;
        }
        public void Buy( decimal share, decimal price )
        {
            Share += share;
            Liquidity -= share * price;
        }
        public void Pay( decimal price )
        {
            Liquidity -= price;
        }
    }
}
