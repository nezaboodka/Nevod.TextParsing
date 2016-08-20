// Free open-source Sharik library - http://code.google.com/p/sharik/

using System;
using System.Threading;

namespace Sharik.Threading
{
    public class ThreadVariable<T> where T: new()
    {
        private readonly bool fAutoCreateInstances;
        private T[] fThreadLocalValues;

        public ThreadVariable(bool autoCreateInstances)
        {
            fAutoCreateInstances = autoCreateInstances;
            fThreadLocalValues = new T[0];
        }
 
        public T Value
        {
            get
            {
                int id = Thread.CurrentThread.ManagedThreadId;
                AcquireVariable(id);
                return fThreadLocalValues[id];
            }
            set
            {
                int id = Thread.CurrentThread.ManagedThreadId;
                AcquireVariable(id);
                fThreadLocalValues[id] = value;
            }
        }

        private void AcquireVariable(int threadId)
        {
            if (fThreadLocalValues.Length <= threadId)
            {
                lock (this)
                {
                    if (fThreadLocalValues.Length <= threadId)
                    {
                        var a = new T[threadId + 1];
                        fThreadLocalValues.CopyTo(a, 0);
                        if (fAutoCreateInstances)
                            for (int i = fThreadLocalValues.Length; i < a.Length; i++)
                                a[i] = new T();
                        fThreadLocalValues = a;
                    }
                }
            }
        }
    }
}
