using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public class PositionBull : Position
    {
        public override void Start( decimal price, IWallet wallet, IBroker broker )
        {
            decimal share = Share (wallet, price);
            decimal fee = broker.GetBuyFee (wallet.StockId, share);
            CurrentShare = Math.Max (0, share - fee);
            wallet.Buy (CurrentShare, price);
            wallet.Pay (fee);
        }

        public override void Stop( decimal price, IWallet wallet, IBroker broker )
        {
            decimal fee = broker.GetSellFee (wallet.StockId, CurrentShare);
            wallet.Sell (CurrentShare, price);
            wallet.Pay (fee);
        }
    }
}
