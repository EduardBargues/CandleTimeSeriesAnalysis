using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public partial class Position
    {
        public static PositionBuilder Builder => new PositionBuilder ( );

        public class PositionBuilder
        {
            protected IPosition Position;

            public PositionStopPricesBuilder BearEnter()
            {
                Position = new PositionBear ( );
                return new PositionStopPricesBuilder ( );
            }
            public PositionStopPricesBuilder BullEnter()
            {
                Position = new PositionBull ( );
                return new PositionStopPricesBuilder ( );
            }

            public PositionStopConditionBuilder Exit => new PositionStopConditionBuilder ( );

            public PositionBuilder WithShare( Func<IWallet, double, double> shareFunction )
            {
                Position.Share = shareFunction;
                return this;
            }

            public IPosition Build() => Position;
        }
    }
}
