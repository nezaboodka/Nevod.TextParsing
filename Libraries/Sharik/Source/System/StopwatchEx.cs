// Free open-source Sharik library - http://code.google.com/p/sharik/

using System;
using System.Diagnostics;

namespace Sharik
{
    public class StopwatchEx : IDisposable
    {
        public StopwatchEx() : this(null)
        {
        }

        public StopwatchEx(string hint)
        {
            if (hint == null)
            {
                var m = new StackTrace().GetFrame(2).GetMethod();
                hint = m.ReflectedType.Name + "." + m.Name;
            }
            _Hint = hint;
            _Stopwatch = new Stopwatch();
            _Stopwatch.Start();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~StopwatchEx()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _Stopwatch.Stop();
                Console.WriteLine("{0} - {1}: {2} ms", DateTime.UtcNow.ToString("o"), _Hint, _Stopwatch.ElapsedMilliseconds);
            }
        }

        private string _Hint;
        private Stopwatch _Stopwatch;
    }

    public static class StopwatchExtension
    {
        public static double Divider = ((double)Stopwatch.Frequency) / 1000.0;

        public static double ElapsedPreciseMilliseconds(this Stopwatch sw)
        {
            return sw.ElapsedTicks / Divider;
        }

        public static long ElapsedMicroseconds(this Stopwatch sw)
        {
            return sw.ElapsedTicks * 1000000 / Stopwatch.Frequency;
        }
    }
}
