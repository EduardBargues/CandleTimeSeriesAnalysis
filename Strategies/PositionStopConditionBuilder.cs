using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public partial class Position
    {
        public class PositionStopConditionBuilder : PositionBuilder
        {
            public PositionStopConditionBuilder When( Predicate<DateTime> predicate )
            {
                Position.StopCondition = predicate;
                return this;
            }

            public PositionStopConditionBuilder And( Predicate<DateTime> predicate )
            {
                Position.StopCondition = Position.StopCondition.And ( predicate );
                return this;
            }

            public PositionStopConditionBuilder Or( Predicate<DateTime> predicate )
            {
                Position.StopCondition = Position.StopCondition.Or ( predicate );
                return this;
            }
        }
    }
}
