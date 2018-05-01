using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public class TraderStreamer : ITradeStreamer
    {
        // FIELDS
        readonly List<Trade> trades;
        // PROPERTIES

        // CONSTRUCTORS
        public TraderStreamer( List<Trade> trades ) => this.trades = trades;

        // STATIC CONSTRUCTORS

        // PUBLIC METHODS
        public IEnumerable<Trade[]> Stream() => trades
            .GroupAdjacent ( trade => trade.Instant )
            .Select ( ts => ts.ToArray ( ) );

        //PRIVATE METHODS
    }
}