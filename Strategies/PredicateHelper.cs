using System;
using System.Linq;

namespace CandleTimeSeriesAnalysis.Strategies
{
    public static class PredicateHelper
    {
        public static Predicate<T> And<T>(params Predicate<T>[] predicates) =>
            item => predicates.All(predicate => predicate(item));

        public static Predicate<T> Or<T>(params Predicate<T>[] predicates) =>
            item => predicates.Any(predicate => predicate(item));
    };
}
