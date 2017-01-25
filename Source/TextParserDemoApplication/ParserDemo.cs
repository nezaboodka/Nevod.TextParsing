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
            long fileSize = new FileInfo(fileName).Length;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Parser.ParsePlainText(text);
            stopwatch.Stop();
            long elapsedPlainText = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();
            Parser.ParseXhtmlText(text);
            stopwatch.Stop();;
            long elapsedXhtml = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"{fileSize} bytes");
            Console.WriteLine($"As plain text: {elapsedPlainText} ms");
            Console.WriteLine($"As xhtml: {elapsedXhtml} ms");
        }
    }
}
