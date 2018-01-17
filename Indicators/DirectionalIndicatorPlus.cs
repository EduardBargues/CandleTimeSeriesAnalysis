using CommonUtils;
using System;

namespace CandleTimeSeriesAnalysis.Indicators
{
    public class DirectionalIndicatorPlus : Indicator
    {
        public DirectionalIndicatorPlus(Func<CandleTimeSeries, DateTime, double> function) : base(function)
        {

        }

        public static DirectionalIndicatorPlus Create(int periods)
        {
            AverageDirectionalMovementPlus admPlus = AverageDirectionalMovementPlus.Create(periods);
            AverageTrueRange atr = AverageTrueRange.Create(periods);

            double Function(CandleTimeSeries series, DateTime instant)
            {
                return admPlus[series, instant].DivideBy(atr[series, instant]);
            }

            DirectionalIndicatorPlus result = new DirectionalIndicatorPlus(Function);
            return result;
        }
    }
}
