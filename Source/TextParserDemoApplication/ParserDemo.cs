using System;
using System.Diagnostics;
using System.IO;
using TextParser;

namespace TextParserDemoApplication
{
    public class ParserDemo
    {
        private const int IterationsCount = 5;

        static int Main(string[] args)
        {
            if (args.Length == 1)
            {
                string fileName = args[0];
                Console.WriteLine($"File: {fileName}");
                try
                {
                    string text = File.ReadAllText(fileName);
                    long fileSize = new FileInfo(fileName).Length;
                    Console.WriteLine($"Size: {fileSize} bytes");
                    Console.WriteLine();
                    for (int i = 0; i < IterationsCount; i++)
                    {
                        Console.WriteLine($"Iteration: {i + 1}");
                        MeasurePlainTextParsing(text);
                        MeasureXhtmlParsing(text);
                        Console.WriteLine();
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Error: File not found");
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("Error: File not specified");
                return -1;
            }
            return 0;
        }

        // Static internal

        private static void MeasurePlainTextParsing(string text)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            PlainTextParser.Parse(text);
            stopwatch.Stop();
            Console.WriteLine($"As plain text: {stopwatch.ElapsedMilliseconds} ms");
        }

        private static void MeasureXhtmlParsing(string text)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            XhtmlParser.Parse(text);
            stopwatch.Stop();
            Console.WriteLine($"As xhtml: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
