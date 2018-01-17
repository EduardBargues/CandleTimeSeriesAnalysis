using CommonUtils;
using System;

namespace CandleTimeSeriesAnalysis.Indicators
{
    public class DirectionalIndicatorMinus : Indicator
    {
        public DirectionalIndicatorMinus(Func<CandleTimeSeries, DateTime, double> function) : base(function)
        {

        }

        public static DirectionalIndicatorMinus Create(int periods)
        {
            AverageDirectionalMovementMinus admMinus = AverageDirectionalMovementMinus.Create(periods);
            AverageTrueRange atr = AverageTrueRange.Create(periods);

            double Function(CandleTimeSeries series, DateTime instant)
            {
                return admMinus[series, instant].DivideBy(atr[series, instant]);
            }

            DirectionalIndicatorMinus result = new DirectionalIndicatorMinus(Function);
            return result;
        }
    }
}
