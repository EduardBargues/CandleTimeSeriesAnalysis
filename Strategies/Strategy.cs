using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public partial class Strategy : IStrategy
    {
        public Predicate<DateTime> EnterWhen { get; set; }
        public IPosition Position { get; set; }

        public static StrategyBuilder Builder => new StrategyBuilder ();
    }
}
