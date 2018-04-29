using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public class Broker : IBroker
    {
        private readonly Func<string, decimal, decimal> buyFee;
        private readonly Func<string, decimal, decimal> sellFee;

        public Broker(Func<string, decimal, decimal> buyFee, Func<string, decimal, decimal> sellFee)
        {
            this.buyFee = buyFee;
            this.sellFee = sellFee;
        }

        public decimal GetBuyFee(string stockId, decimal share) => buyFee(stockId, share);
        public decimal GetSellFee(string stockId, decimal share) => sellFee(stockId, share);
    }
}
