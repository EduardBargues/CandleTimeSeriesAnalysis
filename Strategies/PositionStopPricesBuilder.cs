namespace CandleTimeSeriesAnalysis.Strategies
{
    public partial class Position
    {
        public class PositionStopPricesBuilder : PositionBuilder
        {
            public PositionStopPricesBuilder WithLowerStop( decimal lowerPrice )
            {
                Position.LowerStop = lowerPrice;
                return this;
            }

            public PositionStopPricesBuilder WithUpperStop( decimal upperPrice )
            {
                Position.UpperStop = upperPrice;
                return this;
            }
        }
    }
}
