namespace CandleTimeSeriesAnalysis.Strategies
{
    public class TradeActionInfoUpdatePosition : TradeActionInfo
    {
        public double NewLowerStop { get; set; }
        public double NewUpperStop { get; set; }
    }
}
