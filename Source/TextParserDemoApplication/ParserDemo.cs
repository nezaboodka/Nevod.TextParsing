using System;
using System.Diagnostics;
using System.IO;
using TextParser;
using TextParser.Common;

namespace TextParserDemoApplication
{
    class ParserDemo
    {
        static void Main(string[] args)
        {
            string fileName = args[0];
            string text = File.ReadAllText(fileName);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Parser.ParsePlainText(text);
            stopwatch.Stop();
            FileInfo fileInfo = new FileInfo(fileName);           
            Console.WriteLine($"{fileInfo.Length} bytes, {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
