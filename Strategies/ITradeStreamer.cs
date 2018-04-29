using System.Collections.Generic;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public interface ITradeStreamer
    {
        IEnumerable<Trade[]> Stream();
    }
}