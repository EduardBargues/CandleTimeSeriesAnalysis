using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public interface IPosition : ICloneable
    {
        decimal EntryPrice { get; }
        Func<IWallet, decimal, decimal> Share { get; set; }
        decimal UpperStop { get; set; }
        decimal LowerStop { get; set; }
        Predicate<DateTime> StopCondition { get; set; }
        void Start( decimal price, IWallet wallet, IBroker broker );
        void Stop( decimal price, IWallet wallet, IBroker broker );
        bool ReachesStops( decimal price );
    }
}