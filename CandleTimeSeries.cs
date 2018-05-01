using CommonUtils;
using MoreLinq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CandleTimeSeriesAnalysis
{
    public class CandleTimeSeries
    {
        // FIELDS
        readonly Dictionary<DateTime, Candle> candlesByDate = new Dictionary<DateTime, Candle> ( );
        readonly Dictionary<Candle, int> indicesByCandle = new Dictionary<Candle, int> ( );

        // PROPERTIES
        public IEnumerable<DateTime> Dates => candlesByDate.Keys;
        public string Name { get; set; }
        public List<Candle> Candles { get; }

        // CONSTRUCTORS
        public CandleTimeSeries()
        { }
        public CandleTimeSeries( IEnumerable<Candle> candles )
        {
            Candles = candles
                .OrderBy ( candle => candle.Start )
                .ToList ( );

            Candles
                .ForEach ( ( candle, index ) => {
                    candlesByDate.Add ( candle.Start, candle );
                    indicesByCandle.Add ( candle, index );
                } );
        }

        // STATIC CONSTRUCTORS
        public static CandleTimeSeries Create( IEnumerable<Candle> candles ) => new CandleTimeSeries ( candles );

        // PUBLIC METHODS
        public Candle GetCandle( DateTime date ) => candlesByDate[date];
        public Candle GetCandle( int index ) => index >= 0 && index < Candles.Count ? Candles[index] : null;
        public Candle GetLastCandle() => GetCandle ( Candles.Count - 1 );
        public Candle this[DateTime date] => GetCandle ( date );
        public Candle this[int index] => GetCandle ( index );

        public int GetIndex( Candle candle ) => indicesByCandle[candle];
        public int GetIndex( DateTime date ) => GetIndex ( candlesByDate[date] );

        public bool ContainsCandleAt( DateTime date ) => candlesByDate.ContainsKey ( date );
        public bool HasCandles() => Dates.Any ( );

        public void AddCandle( Candle newCandle, bool forceSort = false )
        {
            Candles.Add ( newCandle );

            if (forceSort) {
                Candles.Sort ( );
                candlesByDate.Clear ( );
                indicesByCandle.Clear ( );
                Candles
                    .ForEach ( ( candle, index ) => {
                        candlesByDate.Add ( candle.Start, candle );
                        indicesByCandle.Add ( candle, index );
                    } );
            } else {
                candlesByDate.Add ( newCandle.Start, newCandle );
                indicesByCandle.Add ( newCandle, Candles.Count );
            }
        }
        public static void Plot( CandleTimeSeriesPlotInfo info )
        {
            PlotView view = GetPlotView ( info );

            Form form = new Form ( );
            form.Controls.Add ( view );
            form.ShowDialog ( );
        }
        public static PlotView GetPlotView( CandleTimeSeriesPlotInfo info
            , Action<object, AxisChangedEventArgs> onAxisChangedMethod = null )
        {
            List<HighLowItem> items = info.Series.Candles
                .OrderBy ( candle => candle.Start )
                .Select ( candle => new HighLowItem ( ) {
                    X = DateTimeAxis.ToDouble ( candle.Start ),
                    Open = (double)candle.Open,
                    Close = (double)candle.Close,
                    High = (double)candle.Max,
                    Low = (double)candle.Min,
                } )
                .ToList ( );
            CandleStickSeries series = new CandleStickSeries {
                RenderInLegend = true,
                Title = info.Title,
                Background = OxyColor.FromArgb (
                    info.BackgroundColor.A,
                    info.BackgroundColor.R,
                    info.BackgroundColor.G,
                    info.BackgroundColor.B ),
                //CandleWidth = info.CandleWidth,
                IsVisible = true,
                DecreasingColor = OxyColor.FromArgb (
                    info.DecreasingColor.A,
                    info.DecreasingColor.R,
                    info.DecreasingColor.G,
                    info.DecreasingColor.B ),
                IncreasingColor = OxyColor.FromArgb (
                    info.IncreasingColor.A,
                    info.IncreasingColor.R,
                    info.IncreasingColor.G,
                    info.IncreasingColor.B ),
                Color = OxyColor.FromArgb (
                    info.BackgroundColor.A,
                    info.BackgroundColor.R,
                    info.BackgroundColor.G,
                    info.BackgroundColor.B ),
                ItemsSource = items,
                //Hollow = info.PositiveCandlesHollow,
            };

            PlotModel model = new PlotModel ( );
            model.Axes.Clear ( );
            DateTimeAxis timeAxis = new DateTimeAxis ( );
            if (onAxisChangedMethod != null)
                timeAxis.AxisChanged += onAxisChangedMethod.Invoke;
            model.Axes.Add ( timeAxis );
            model.Series.Clear ( );
            model.Series.Add ( series );
            PlotView view = new PlotView {
                Model = model,
                Dock = DockStyle.Fill,
            };
            return view;
        }

        // PRIVATE METHODS
        public static IEnumerable<Candle> FromTrades(
            IEnumerable<Trade> trades,
            TimeSpan? candleDuration = null,
            int? numberOfTradesPerCandle = null,
            double? volumePerCandle = null )
        {

            if (candleDuration == null &&
                numberOfTradesPerCandle == null &&
                volumePerCandle == null)
                throw new InvalidOperationException ( "At least one of candleDuration, numberOfTradesPerCandle or valumePerCandle must be defined." );

            if (candleDuration != null)
                return GetCandles ( trades, candleDuration.Value );
            if (numberOfTradesPerCandle != null)
                return GetCandles ( trades, numberOfTradesPerCandle.Value );

            return GetCandles ( trades, volumePerCandle.Value );
        }
        static IEnumerable<Candle> GetCandles( IEnumerable<Trade> trades, double volumePerCandle )
        {
            List<Trade> sortedTrades = trades
                .OrderBy ( trade => trade.Instant )
                .ToList ( );
            if (sortedTrades.Any ( )) {
                Trade firstTrade = sortedTrades.First ( );
                Candle candle = new Candle { Start = firstTrade.Instant };
                foreach (Trade trade in sortedTrades) {
                    if (trade.Type == TradeType.Buy)
                        candle.BuyVolume += trade.Volume;
                    if (trade.Type == TradeType.Sell)
                        candle.SellVolume += trade.Volume;
                    if (candle.Max < trade.Price)
                        candle.Max = trade.Price;
                    if (candle.Min > trade.Price)
                        candle.Min = trade.Price;
                    if (candle.Volume >= volumePerCandle) {
                        candle.Duration = trade.Instant - candle.Start;
                        candle.Close = trade.Price;
                        yield return candle;
                        candle = new Candle { Start = trade.Instant };
                    }
                }
            }
        }

        static IEnumerable<Candle> GetCandles( IEnumerable<Trade> trades, int numberOfTradesPerCandle ) => trades
                .OrderBy ( trade => trade.Instant )
                .Select ( ( trade, index ) => new { Trade = trade, Position = index + 1 } )
                .GroupBy ( anonymous => anonymous.Position / numberOfTradesPerCandle, anonymous => anonymous.Trade )
                .Select ( grouping => {
                    Candle candle = Candle.FromTrades ( grouping );
                    Trade firstTrade = grouping.First ( );
                    candle.Start = firstTrade.Instant;
                    candle.Duration = grouping.Last ( ).Instant - firstTrade.Instant;
                    return candle;
                } );
        static IEnumerable<Candle> GetCandles( IEnumerable<Trade> trades, TimeSpan candleDuration )
        {
            List<Trade> sortedTrades = trades
                .OrderBy ( trade => trade.Instant )
                .ToList ( );
            if (sortedTrades.Any ( )) {
                DateTime firstDate = sortedTrades.First ( ).Instant;
                Func<Trade, int> f = t => {
                    DateTime date = t.Instant;
                    TimeSpan span = date - firstDate;
                    double factor = span.DivideBy ( candleDuration );
                    return (int)factor;
                };
                foreach (IGrouping<int, Trade> grouping in sortedTrades.GroupBy ( trade => f.Invoke ( trade ) )) {
                    Candle candle = Candle.FromTrades ( grouping );
                    Trade firstTrade = grouping.First ( );
                    int scale = f.Invoke ( firstTrade );
                    candle.Start = firstDate + candleDuration.MultiplyBy ( scale );
                    candle.Duration = candleDuration;
                    yield return candle;
                }
            }
        }
    }
}