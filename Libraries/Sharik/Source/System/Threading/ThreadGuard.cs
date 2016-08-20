// Free open-source Sharik library - http://code.google.com/p/sharik/

using System;
using System.Threading;

namespace Sharik.Threading
{
    public struct ThreadGuard
    {
        private Thread Owner;

        public void CheckThread()
        {
            var owner = Interlocked.Exchange(ref Owner, Thread.CurrentThread);
            if (owner != default(Thread) && owner != Thread.CurrentThread)
                throw new InvalidOperationException("possible race condition detected");
        }
    }
}
