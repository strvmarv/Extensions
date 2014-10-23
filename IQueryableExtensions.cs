using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Extensions
{
    public static class IQueryableExtensions
    {
        public static bool Exists<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return source.Count(predicate) > 0;
        }
    }
}
