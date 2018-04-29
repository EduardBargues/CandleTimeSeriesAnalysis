using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public abstract partial class Position : IPosition
    {
        protected decimal CurrentShare;

        public decimal EntryPrice { get; set; }
        public Func<IWallet, decimal, decimal> Share { get; set; }
        public decimal UpperStop { get; set; }
        public decimal LowerStop { get; set; }
        public Predicate<DateTime> StopCondition { get; set; }

        public abstract void Start( decimal price, IWallet wallet, IBroker broker );
        public abstract void Stop( decimal price, IWallet wallet, IBroker broker );
        public bool ReachesStops( decimal price ) => price >= EntryPrice + UpperStop ||
                                                     price <= EntryPrice - LowerStop;
    }
}
