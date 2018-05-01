using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public interface IPosition : ICloneable
    {
        double EntryPrice { get; }
        Func<IWallet, double, double> Share { get; set; }
        double UpperStop { get; set; }
        double LowerStop { get; set; }
        Predicate<DateTime> StopCondition { get; set; }
        void Start( double price, IWallet wallet, IBroker broker );
        void Stop( double price, IWallet wallet, IBroker broker );
        bool ReachesStops( double price );
    }
}