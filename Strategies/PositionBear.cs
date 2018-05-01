namespace CandleTimeSeriesAnalysis.Strategies
{
    public class PositionBear : Position
    {
        public override void Start( double price, IWallet wallet, IBroker broker )
        {
            double shareToSell = Share ( wallet, price );
            double fee = broker.GetSellFee ( wallet.StockId, shareToSell );
            wallet.Sell ( shareToSell, price );
            wallet.Pay ( fee );
            CurrentShare = shareToSell;
        }

        public override void Stop( double price, IWallet wallet, IBroker broker )
        {
            double fee = broker.GetBuyFee ( wallet.StockId, CurrentShare );
            wallet.Buy ( CurrentShare, price );
            wallet.Pay ( fee );
        }

        public override object Clone() => new PositionBear ( ) {
            StopCondition = StopCondition,
            Share = Share,
            LowerStop = LowerStop,
            EntryPrice = EntryPrice,
            UpperStop = UpperStop,
            CurrentShare = CurrentShare,
        };
    }
}
