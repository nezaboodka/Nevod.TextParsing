// Free open-source Sharik library - http://code.google.com/p/sharik/

using System;
using System.Collections.Generic;

namespace Sharik.Threading
{
    public class LockFreeDictionary<TKey, TValue> where TValue: class
    {
        // NOT IMPLEMENTED YET

        //public Holder<TValue> this[TKey key]
        //{
        //    get { return Acquire(key); }
        //}

        //private Holder<TValue> Acquire(TKey key)
        //{
        //    var t = fItems;
        //    var result = (Holder<TValue>)null;
        //    if (!t.TryGetValue(key, out result))
        //    {
        //        lock (this)
        //        {
        //            if (t == fItems)
        //            {
        //                t = new T[index + 16];
        //                Array.Copy(fItems, t, fItems.Length);
        //            }
        //            t = fItems;
        //        }
        //    }
        //    return t;
        //}

        //private Dictionary<TKey, Holder<TValue>> fItems = new Dictionary<TKey, Holder<TValue>>();
    }
}
