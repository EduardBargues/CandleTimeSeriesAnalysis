using System;
using System.Collections.Generic;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public interface ITrader
    {
        IEnumerable<ITradeActionInfo> Trade( ITradeStreamer streamer, DateTime startDate );
    }
}