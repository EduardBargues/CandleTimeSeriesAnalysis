using System;
using System.Collections.Generic;
using System.Linq;

namespace CandleTimeSeriesAnalysis
{
    public class Candle
    {
        public DateTime Start { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime End => Start.Add ( Duration );
        public decimal Max { get; set; }
        public decimal Min { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal BuyVolume { get; set; }
        public decimal SellVolume { get; set; }
        public decimal Volume => BuyVolume + SellVolume;
        public bool GoesUp => Open <= Close;
        public bool GoesDown => !GoesUp;
        public decimal Body => Math.Abs ( Open - Close );
        public decimal Range => Max - Min;
        public decimal BodyRangeRatio => Body / (Range > 0 ? Range : 1);

        public static Candle FromTrades( IEnumerable<Trade> trades )
        {
            List<Trade> list = trades
                .ToList ( );

            Candle candle = new Candle {
                Max = list
                    .Max ( trade => trade.Price ),
                Open = list.First ( ).Price,
                Min = list
                    .Min ( trade => trade.Price ),
                Close = list.Last ( ).Price,
                SellVolume = list
                    .Where ( trade => trade.Type == TradeType.Sell )
                    .Sum ( trade => trade.Volume ),
                BuyVolume = list
                    .Where ( trade => trade.Type == TradeType.Buy )
                    .Sum ( trade => trade.Volume )
            };

            return candle;
        }
    }
}
