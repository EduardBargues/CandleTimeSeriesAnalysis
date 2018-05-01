namespace CandleTimeSeriesAnalysis.Strategies
{
    public partial class Position
    {
        public class PositionStopPricesBuilder : PositionBuilder
        {
            public PositionStopPricesBuilder WithLowerStop( double lowerPrice )
            {
                Position.LowerStop = lowerPrice;
                return this;
            }

            public PositionStopPricesBuilder WithUpperStop( double upperPrice )
            {
                Position.UpperStop = upperPrice;
                return this;
            }
        }
    }
}
