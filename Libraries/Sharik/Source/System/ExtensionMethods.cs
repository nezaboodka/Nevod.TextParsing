// Free open-source Sharik library - http://code.google.com/p/sharik/

using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sharik
{
    public static class EnumerableMethods
    {
        public static string ToString<T>(this IEnumerable<T> items, string delimiter)
        {
            return items.ToString("{0}", delimiter);
        }

        public static string ToString<T>(this IEnumerable<T> items, string itemFormat, string delimiter)
        {
            var sb = new StringBuilder();
            foreach (var x in items)
            {
                if (sb.Length > 0)
                    sb.Append(delimiter);
                sb.AppendFormat(itemFormat, x.ToString());
            }
            return sb.ToString();
        }
    }

    public static class CollectionMethods
    {
        public static void ClearAndAdd<T>(this ICollection<T> collection, params T[] items)
        {
            collection.Clear();
            collection.AddRange(items);
        }

        public static void Add<T>(this ICollection<T> collection, params T[] items)
        {
            collection.AddRange(items);
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> from)
        {
            if (from != null)
                foreach (T x in from)
                    collection.Add(x);
        }

        public static void AddRange(this IList collection, IEnumerable from)
        {
            if (from != null)
                foreach (object x in from)
                    collection.Add(x);
        }
    }
}
