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
        Action<TradeActionInfo> tradeActionPerformed;

        public Trader( IWallet wallet, IBroker broker, List<IStrategy> strategies, CandleTimeSeries series, TimeSpan workPeriod )
        {
            this.wallet = wallet;
            this.broker = broker;
            this.strategies = strategies;
            this.series = series;
            this.workPeriod = workPeriod;
        }

        public IEnumerable<TradeActionInfo> Trade( ITradeStreamer streamer, DateTime startDate )
        {
            Candle currentCandle = null;
            IPosition currentPosition = null;

            foreach (Trade[] trades in streamer.Stream ( )) {
                TradeActionInfo info;
                bool positionOpened = currentPosition != null;
                bool newCandleRequired = currentCandle == null ||
                                         trades.Any ( trade => trade.Instant >= currentCandle.End );
                if (newCandleRequired) {
                    (currentPosition, info) = positionOpened
                        ? ClosePositionIfRequired ( currentPosition, trades, true )
                        : OpenPositionIfRequired ( series );
                    yield return info;
                    (currentPosition, info) = UpdatePosition ( currentPosition, series );
                    yield return info;
                    currentCandle = GetNewCandle ( trades, currentCandle?.End ?? startDate );
                    series.AddCandle ( currentCandle );
                } else {
                    (currentPosition, info) = ClosePositionIfRequired ( currentPosition, trades, false );
                    yield return info;
                    UpdateCandle ( currentCandle, trades );
                }
            }
        }

        (IPosition, TradeActionInfo) OpenPositionIfRequired( CandleTimeSeries candleSeries )
        {
            Candle lastCandle = candleSeries.GetLastCandle ( );
            if (lastCandle == null) {
                return (null, null);
            }

            IPosition position = null;
            TradeActionInfoOpenPosition info = null;

            IStrategy enterStrategy = strategies
                .FirstOrDefault ( strategy => strategy.EnterWhen ( lastCandle.End ) );
            if (enterStrategy != null) {
                position = enterStrategy.Position;
                position.Start ( lastCandle.Close, wallet, broker );
            }

            if (position != null) {
                info = new TradeActionInfoOpenPosition ( ) {
                    Price = lastCandle.Close,
                    UpperStop = position.UpperStop,
                    LowerStop = position.LowerStop,
                    StockId = candleSeries.Name,
                    WalletLiquidity = wallet.Liquidity,
                    WalletShare = wallet.Share,
                    Instant = lastCandle.End
                };
            }

            return (position, info);
        }
        (IPosition, TradeActionInfo) UpdatePosition( IPosition position, CandleTimeSeries candleSeries )
        {
            Candle lastCandle = candleSeries.GetLastCandle ( );
            if (position == null ||
                lastCandle == null) {
                return (position, null);
            }
            IPosition updatedPosition = (IPosition)position.Clone ( );
            updatedPosition.LowerStop = lastCandle.Close - (position.EntryPrice - position.LowerStop);

            TradeActionInfoUpdatePosition info = new TradeActionInfoUpdatePosition ( ) {
                Price = lastCandle.Close,
                StockId = candleSeries.Name,
                WalletLiquidity = wallet.Liquidity,
                WalletShare = wallet.Share,
                NewLowerStop = updatedPosition.LowerStop,
                NewUpperStop = updatedPosition.UpperStop,
                Instant = lastCandle.End
            };

            return (updatedPosition, info);
        }
        (IPosition, TradeActionInfo) ClosePositionIfRequired( IPosition position, Trade[] trades, bool newCandleRequired )
        {
            double? price = null;
            Trade limitTrade = trades
                .FirstOrDefault ( trade => position.ReachesStops ( trade.Price ) );
            if (!newCandleRequired &&
                limitTrade != null) {
                price = limitTrade.Price >= position.UpperStop
                    ? position.UpperStop
                    : position.LowerStop;
            } else if (newCandleRequired &&
                       position.StopCondition ( trades.Last ( ).Instant )) {
                price = trades.Last ( ).Price;
            }

            if (price.HasValue) {
                position.Stop ( price.Value, wallet, broker );
                return (null, new TradeActionInfoClosePosition ( ) {
                    Price = price.Value,
                    StockId = wallet.StockId,
                    WalletLiquidity = wallet.Liquidity,
                    WalletShare = wallet.Share,
                    Instant = trades.First ( ).Instant,
                });
            } else {
                return (position, null);
            }
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
