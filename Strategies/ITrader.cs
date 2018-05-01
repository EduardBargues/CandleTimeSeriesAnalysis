using System;
using System.Collections.Generic;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public interface ITrader
    {
        IEnumerable<TradeActionInfo> Trade( ITradeStreamer streamer, DateTime startDate );
    }
}