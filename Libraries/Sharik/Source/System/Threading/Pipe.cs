// Free open-source Sharik library - http://code.google.com/p/sharik/

using System;
using System.Collections.Generic;
using System.Threading;

namespace Sharik.Threading
{
    public class Pipe<TItem> : IDisposable
    {
        private int fCapacity;
        private Queue<TItem> fQueue;

        public Pipe(int capacity)
        {
            fCapacity = capacity;
            fQueue = new Queue<TItem>();
        }

        public void GetCapacityAndCount(out int capacity, out int count)
        {
            lock (fQueue)
            {
                capacity = fCapacity;
                count = fQueue.Count;
            }
        }

        public bool Write(TItem item) // timeout = Timeout.Infinite
        {
            return Write(item, Timeout.Infinite);
        }

        public bool Read(out TItem item) // timeout = Timeout.Infinite
        {
            return Read(out item, Timeout.Infinite, default(TItem));
        }

        public bool Write(TItem item, int timeout)
        {
            var noTimeout = true;
            lock (fQueue)
            {
                while (fCapacity > 0 && noTimeout && fQueue.Count >= fCapacity)
                    noTimeout = Monitor.Wait(fQueue, timeout);
                if (fCapacity == 0)
                    throw new ArgumentException("cannot write to closed pipe");
                if (noTimeout)
                {
                    fQueue.Enqueue(item);
                    Monitor.PulseAll(fQueue);
                }
            }
            return noTimeout;
        }

        public bool Read(out TItem item, int timeout, TItem timeoutItem)
        {
            lock (fQueue)
            {
                while (fQueue.Count == 0 && fCapacity > 0)
                {
                    if (!Monitor.Wait(fQueue, timeout))
                    {
                        item = timeoutItem;
                        return true;
                    }
                }
                if (fQueue.Count > 0)
                {
                    item = fQueue.Dequeue();
                    Monitor.PulseAll(fQueue);
                    return true;
                }
            }
            item = default(TItem);
            return false;
        }

        public Reader<TItem> GetReader(int moveNextTimeout, TItem timeoutIndicator)
        {
            return delegate(out TItem item)
            {
                return Read(out item, moveNextTimeout, timeoutIndicator);
            };
        }

        public bool IsClosed()
        {
            lock (fQueue)
                return fCapacity == 0;
        }

        public void Close()
        {
            lock (fQueue)
            {
                if (fCapacity != 0)
                {
                    fCapacity = 0;
                    Monitor.PulseAll(fQueue); // provoke exceptions in all writing threads that wait
                }
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
