using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public class Broker : IBroker
    {
        readonly Func<string, double, double> buyFee;
        readonly Func<string, double, double> sellFee;

        public Broker( Func<string, double, double> buyFee, Func<string, double, double> sellFee )
        {
            this.buyFee = buyFee;
            this.sellFee = sellFee;
        }

        public double GetBuyFee( string stockId, double share ) => buyFee ( stockId, share );
        public double GetSellFee( string stockId, double share ) => sellFee ( stockId, share );
    }
}
