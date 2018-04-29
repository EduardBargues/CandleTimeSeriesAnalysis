namespace CandleTimeSeriesAnalysis.Strategies
{
    public class PositionBear : Position
    {
        public override void Start( decimal price, IWallet wallet, IBroker broker )
        {
            decimal shareToSell = Share (wallet, price);
            decimal fee = broker.GetSellFee (wallet.StockId, shareToSell);
            wallet.Sell (shareToSell, price);
            wallet.Pay (fee);
            CurrentShare = shareToSell;
        }

        public override void Stop( decimal price, IWallet wallet, IBroker broker )
        {
            decimal fee = broker.GetBuyFee (wallet.StockId, CurrentShare);
            wallet.Buy (CurrentShare, price);
            wallet.Pay (fee);
        }
    }
}
