using System;
using System.Collections.Generic;
using System.Linq;

namespace Util
{
    public static class LinqExtensions
    {
        public static ISet<TSource> ToSet<TSource>(this IEnumerable<TSource> source) 
            => new HashSet<TSource>(source ?? new TSource[0]);

        public static IEnumerable<TResult> SelectNotNull<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TResult> selector) => source.Select(selector).WhereNotNull();

        public static IEnumerable<TSource> WhereNotNull<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate) => source.Where(predicate).WhereNotNull();

        public static IEnumerable<TSource> WhereNotNull<TSource>(this IEnumerable<TSource> source) 
            => source.Where(it => it != null);
    }
}