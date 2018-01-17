using CommonUtils;
using System;

namespace CandleTimeSeriesAnalysis.Indicators
{
    public class DirectionalMovementIndex : Indicator
    {
        public DirectionalMovementIndex(Func<CandleTimeSeries, DateTime, double> function) : base(function)
        {

        }

        public static DirectionalMovementIndex Create(int periods)
        {
            DirectionalIndicatorPlus diPlus = DirectionalIndicatorPlus.Create(periods);
            DirectionalIndicatorMinus diMinus = DirectionalIndicatorMinus.Create(periods);
            double Function(CandleTimeSeries series, DateTime instant)
            {
                double diDiff = Math.Abs(diPlus[series, instant] - diMinus[series, instant]);
                double diSum = diPlus[series, instant] + diMinus[series, instant];
                double dx = diDiff.DivideBy(diSum);
                return dx;
            }

            DirectionalMovementIndex index = new DirectionalMovementIndex(Function);
            return index;
        }
    }
}
