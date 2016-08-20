// Free open-source Sharik library - http://code.google.com/p/sharik/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sharik
{
    public class MultiComparison<T> : Collection<Comparison<T>>, IComparer<T>
    {
        public MultiComparison(params Comparison<T>[] comparisons)
        {
            this.AddRange(comparisons);
        }

        public int Compare(T x, T y)
        {
            var t = Count - 1;
            return Compare(x, y, ref t);
        }

        public int Compare(T x, T y, ref int level)
        {
            int result = 0, count = level + 1, i = 0;
            while (i < count && result == 0)
                result = this[i++](x, y);
            level = i - 1;
            return result;
        }
    }
}
