using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public interface IStrategy
    {
        Predicate<DateTime> EnterWhen { get; }
        IPosition Position { get; }
    }
}
