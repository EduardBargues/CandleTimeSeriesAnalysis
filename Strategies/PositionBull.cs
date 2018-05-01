using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public class PositionBull : Position
    {
        public override void Start( double price, IWallet wallet, IBroker broker )
        {
            double share = Share ( wallet, price );
            double fee = broker.GetBuyFee ( wallet.StockId, share );
            CurrentShare = Math.Max ( 0, share - fee );
            wallet.Buy ( CurrentShare, price );
            wallet.Pay ( fee );
        }

        public override void Stop( double price, IWallet wallet, IBroker broker )
        {
            double fee = broker.GetSellFee ( wallet.StockId, CurrentShare );
            wallet.Sell ( CurrentShare, price );
            wallet.Pay ( fee );
        }

        public override object Clone() => new PositionBull ( ) {
            StopCondition = StopCondition,
            Share = Share,
            EntryPrice = EntryPrice,
            LowerStop = LowerStop,
            UpperStop = UpperStop,
            CurrentShare = CurrentShare,
        };
    }
}
