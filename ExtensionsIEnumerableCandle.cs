using System.Collections.Generic;

namespace CandleTimeSeriesAnalysis
{
    public static class ExtensionsIEnumerableCandle
    {
        public static CandleTimeSeries ToCandleTimeSeries(this IEnumerable<Candle> candles, string name = "")
        {
            return new CandleTimeSeries(candles)
            {
                Name = name,
            };
        }
    }
}
