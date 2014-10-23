using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class EnumerableExtensions
    {
        public static int SequenceHashCode<T>(this IEnumerable<T> seq)
        {
            unchecked
            {
                return seq != null ? seq.Aggregate(0, (sum, obj) => sum + obj.GetHashCode()) : 0;
            }
        }
    }
}
