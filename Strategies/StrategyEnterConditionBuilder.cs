using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public partial class Strategy
    {
        public class StrategyEnterConditionBuilder : StrategyBuilder
        {
            public StrategyEnterConditionBuilder When( Predicate<DateTime> predicate )
            {
                Strategy.EnterWhen = predicate;
                return this;
            }
            public StrategyEnterConditionBuilder And( Predicate<DateTime> predicate )
            {
                Strategy.EnterWhen = Strategy.EnterWhen.And (predicate);
                return this;
            }
            public StrategyEnterConditionBuilder Or( Predicate<DateTime> predicate )
            {
                Strategy.EnterWhen = Strategy.EnterWhen.Or (predicate);
                return this;
            }
            public StrategyBuilder WithPosition( IPosition position )
            {
                Strategy.Position = position;
                return this;
            }
        }
    }
}
