// Free open-source Sharik library - http://code.google.com/p/sharik/

using System;
using Sharik.Text;
using Sharik.Threading;
using Sharik.Xml;
using Sharik.Security;

namespace Sharik.UnitTests
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("\n\n*** N*DEF Sample ***\n");
            //NdefSample.Main();
            Console.WriteLine("\n\n*** Pipeline Sample ***\n");
            PipelineSample.Main();
            //Console.WriteLine("\n\n*** Format Sample ***\n");
            //FormatSample.Main();
            //Console.WriteLine("\n\n*** XmlPatch Sample ***\n");
            //XmlPatchSample.Main();
            //Console.WriteLine("\n\n*** AccessToken Sample ***\n");
        }
    }
}
