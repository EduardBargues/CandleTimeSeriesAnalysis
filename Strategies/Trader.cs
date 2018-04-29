using System;
using System.Collections.Generic;
using System.Linq;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public class Trader : ITrader
    {
        readonly IWallet wallet;
        readonly IBroker broker;
        readonly List<IStrategy> strategies;
        readonly CandleTimeSeries series;
        readonly TimeSpan workPeriod;

        public Trader( IWallet wallet, IBroker broker, List<IStrategy> strategies, CandleTimeSeries series, TimeSpan workPeriod )
        {
            this.wallet = wallet;
            this.broker = broker;
            this.strategies = strategies;
            this.series = series;
            this.workPeriod = workPeriod;
        }
        public IEnumerable<ITradeActionInfo> Trade( ITradeStreamer streamer, DateTime startDate )
        {
            Candle currentCandle = null;
            IPosition currentPosition = null;

            foreach (Trade[] trades in streamer.Stream ( )) {
                bool positionOpened = currentPosition != null;
                bool newCandleRequired = currentCandle == null ||
                                         trades.Any ( trade => trade.Instant >= currentCandle.End );
                if (newCandleRequired) {
                    currentPosition = positionOpened
                        ? ClosePositionIfRequired ( currentPosition, trades, true )
                        : OpenPositionIfRequired ( series );
                    currentPosition = UpdatePosition ( currentPosition, series );

                    currentCandle = GetNewCandle ( trades, currentCandle?.End ?? startDate );
                    series.AddCandle ( currentCandle );
                } else {
                    currentPosition = ClosePositionIfRequired ( currentPosition, trades, false );
                    UpdateCandle ( currentCandle, trades );
                }
            }
        }

        IPosition OpenPositionIfRequired( CandleTimeSeries candleSeries )
        {
            Candle lastCandle = candleSeries.GetLastCandle ( );
            if (lastCandle == null) {
                return null;
            }

            IPosition position = null;

            IStrategy enterStrategy = strategies
                .FirstOrDefault ( strategy => strategy.EnterWhen ( lastCandle.End ) );
            if (enterStrategy != null) {
                position = enterStrategy.Position;
                position.Start ( lastCandle.Close, wallet, broker );
            }

            return position;
        }
        IPosition UpdatePosition( IPosition position, CandleTimeSeries candleSeries )
        {
            Candle lastCandle = candleSeries.GetLastCandle ( );
            if (position == null ||
                lastCandle == null) {
                return position;
            }
            IPosition updatedPosition = (IPosition)position.Clone ( );
            updatedPosition.LowerStop = lastCandle.Close - (position.EntryPrice - position.LowerStop);

            return updatedPosition;
        }
        IPosition ClosePositionIfRequired( IPosition position, Trade[] trades, bool newCandleRequired )
        {
            Trade limitTrade = trades
                .FirstOrDefault ( trade => position.ReachesStops ( trade.Price ) );
            if (limitTrade != null) {
                position.Stop ( limitTrade.Price, wallet, broker );
                return null;
            }
            if (newCandleRequired &&
                position.StopCondition ( trades.Last ( ).Instant )) {
                position.Stop ( trades.Last ( ).Price, wallet, broker );
                return null;
            }

            return position;
        }
        void UpdateCandle( Candle candle, Trade[] trades )
        {
            candle.Max = Math.Max ( candle.Max, trades.Max ( trade => trade.Price ) );
            candle.Min = Math.Min ( candle.Min, trades.Min ( trade => trade.Price ) );
            candle.Close = trades.Last ( ).Price;
            candle.BuyVolume += trades
                .Where ( trade => trade.Type == TradeType.Buy )
                .Sum ( trade => trade.Volume );
            candle.SellVolume += trades
                .Where ( trade => trade.Type == TradeType.Sell )
                .Sum ( trade => trade.Volume );
        }
        Candle GetNewCandle( Trade[] trades, DateTime start )
        {
            Candle candle = new Candle {
                Start = start,
                Duration = workPeriod,
                Open = trades.First ( ).Price,
            };
            UpdateCandle ( candle, trades );
            return candle;
        }
    }
}
