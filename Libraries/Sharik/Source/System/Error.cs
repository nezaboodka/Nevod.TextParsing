// Free open-source Sharik library - http://code.google.com/p/sharik/

using System;

namespace Sharik
{
    public class Error : Exception
    {
        public Error(Exception inner, string format, params object[] args)
            : base(string.Format(format, args), inner)
        {
        }
    }
}
