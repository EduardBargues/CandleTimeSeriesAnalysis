using CandleTimeSeriesAnalysis.Strategies;
using System;
using System.Collections.Generic;

namespace CandleTimeSeriesAnalysis
{
    internal class Asdf
    {
        private void fdsa()
        {
            CandleTimeSeries series = new CandleTimeSeries () { Name = "BitCoin" };
            ITradeStreamer streamer = new TraderStreamer (new List<Trade> ());
            IWallet wallet = new Wallet ("BitCoin", 0, 1000);
            IBroker broker = new Broker (
                ( stockId, share ) => 5,
                ( stockId, share ) => 5);

            IPosition position = Position.Builder
                .BearEnter ()
                    .WithLowerStop (100)
                    .WithUpperStop (200)
                .WithShare (( wall, price ) => Math.Min (price, (decimal)0.1 * wall.Liquidity))
                .Exit
                    .When (d => true)
                .Build ();

            IStrategy strategy = Strategy.Builder
                .Enter
                    .When (d => true)
                    .And (d => true)
                    .Or (d => true)
                .WithPosition (position)
                .Build ();

            Trader trader = new Trader (
                wallet,
                broker,
                new List<IStrategy> { strategy },
                series,
                TimeSpan.FromMinutes (10));

            trader.Trade (streamer);
        }
    }
}
