using System;

namespace CandleTimeSeriesAnalysis.Indicators
{
    public abstract class Indicator : IIndicator
    {
        readonly Func<CandleTimeSeries, DateTime, double> f;

        protected Indicator()
        {

        }
        protected Indicator( Func<CandleTimeSeries, DateTime, double> function ) => f = function;

        public string Name { get; set; }
        public string ShortName { get; set; }

        public double GetValueAt( CandleTimeSeries series, DateTime date ) => f ( series, date );
        public double this[CandleTimeSeries series, DateTime date] => GetValueAt ( series, date );
    }
}
