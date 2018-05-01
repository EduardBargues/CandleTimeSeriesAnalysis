using CommonUtils;
using System;
using System.Linq;

namespace CandleTimeSeriesAnalysis.Indicators
{
    public class AverageDirectionalMovementIndex : Indicator
    {
        public AverageDirectionalMovementIndex( Func<CandleTimeSeries, DateTime, double> function ) : base ( function )
        {

        }

        public static AverageDirectionalMovementIndex Create(
            int periods,
            int smoothingPeriods )
        {
            DirectionalMovementIndex dmi = DirectionalMovementIndex.Create ( periods );
            double Function( CandleTimeSeries series, DateTime instant )
            {
                Candle candle = series[instant];
                int index = series.GetIndex ( candle );
                Candle[] candles = index.GetIntegersTo ( Math.Max ( 1, index - smoothingPeriods ) )
                    .Select ( idx => series[idx] )
                    .ToArray ( );
                double ema = candles
                    .WeightedAverage ( ( cdl, idx ) => dmi[series, cdl.Start], ( cdl, idx ) => candles.Length - idx );
                return ema;
            }

            AverageDirectionalMovementIndex plus = new AverageDirectionalMovementIndex ( Function );
            return plus;
        }

    }
}
