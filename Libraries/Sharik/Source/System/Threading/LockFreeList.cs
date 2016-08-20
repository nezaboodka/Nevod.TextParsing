// Free open-source Sharik library - http://code.google.com/p/sharik/

using System;
using System.Threading;

namespace Sharik.Threading
{
    public class LockFreeList<T> where T: class
    {
        public LockFreeList() : this(4)
        {
        }

        public LockFreeList(int initialCapacity)
        {
            fItems = new Holder<T>[initialCapacity];
            for (int i = 0; i < initialCapacity; i++)
                fItems[i] = new Holder<T>();
        }

        public Holder<T> this[int index]
        {
            get
            {
                Holder<T>[] t = fItems;
                if (index >= t.Length)
                {
                    lock (this)
                    {
                        if (index >= fItems.Length)
                        {
                            int length = fItems.Length + 1;
                            while (index >= length)
                                length = length * 2;
                            t = new Holder<T>[length];
                            Array.Copy(fItems, t, fItems.Length);
                            for (var i = fItems.Length; i < length; i++)
                                t[i] = new Holder<T>();
                            fItems = t;
                            Thread.MemoryBarrier();
                        }
                    }
                }
                return t[index];
            }
        }

        private Holder<T>[] fItems;
    }
}
