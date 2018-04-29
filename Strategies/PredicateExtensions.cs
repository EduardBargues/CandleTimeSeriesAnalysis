using System;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public static class PredicateExtensions
    {
        public static Predicate<T> And<T>(this Predicate<T> predicate, Predicate<T> other) =>
            item => predicate(item) &&
                    other(item);

        public static Predicate<T> Or<T>(this Predicate<T> predicate, Predicate<T> other) =>
            item => predicate(item) ||
                    other(item);
    }
}
