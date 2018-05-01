using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public abstract partial class Position : IPosition
    {
        protected double CurrentShare;

        public double EntryPrice { get; set; }
        public Func<IWallet, double, double> Share { get; set; }
        public double UpperStop { get; set; }
        public double LowerStop { get; set; }
        public Predicate<DateTime> StopCondition { get; set; }

        public abstract void Start( double price, IWallet wallet, IBroker broker );
        public abstract void Stop( double price, IWallet wallet, IBroker broker );
        public bool ReachesStops( double price ) => price >= EntryPrice + UpperStop ||
                                                     price <= EntryPrice - LowerStop;

        public abstract object Clone();
    }
}
