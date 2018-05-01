using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public abstract class TradeActionInfo
    {
        public string StockId { get; set; }
        public double Price { get; set; }
        public double WalletLiquidity { get; set; }
        public double WalletShare { get; set; }
        public DateTime Instant { get; set; }
    }
}
