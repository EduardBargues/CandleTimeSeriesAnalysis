using System;
using CommonUtils;

namespace CandleTimeSeriesAnalysis.Indicators
{
    public class DirectionalIndicatorPlus : Indicator
    {
        public DirectionalIndicatorPlus(Func<CandleTimeSeries, DateTime, double> function) : base(function)
        {

        }

        public static DirectionalIndicatorPlus Create(int periods, int smoothingPeriods)
        {
            AverageDirectionalMovementPlus admPlus = AverageDirectionalMovementPlus.Create(periods, smoothingPeriods);
            AverageTrueRange atr = AverageTrueRange.Create(smoothingPeriods);

            double Function(CandleTimeSeries series, DateTime instant)
            {
                return admPlus[series, instant].DivideBy(atr[series, instant]);
            }

            DirectionalIndicatorPlus result = new DirectionalIndicatorPlus(Function);
            return result;
        }
    }
}
