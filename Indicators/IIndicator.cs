using System;

namespace CandleTimeSeriesAnalysis.Indicators
{
    public interface IIndicator
    {
        double GetValueAt( CandleTimeSeries series, DateTime instant );
        string Name { get; }
        string ShortName { get; }
    }
}
