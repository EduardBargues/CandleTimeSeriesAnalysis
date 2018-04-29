namespace CandleTimeSeriesAnalysis.Strategies
{
    public partial class Strategy
    {
        public class StrategyBuilder
        {
            protected Strategy Strategy = new Strategy ();

            public StrategyEnterConditionBuilder Enter => new StrategyEnterConditionBuilder ();

            public IStrategy Build() => Strategy;
        }
    }
}
