using System;
using WordExtraction;

namespace WordExtractionDemoApplication
{
    class WordExtractionDemo
    {
        static void Main(string[] args)
        {
            WordExtractor wordExtractor = new WordExtractor();
            foreach (string arg in args)
            {
                foreach (var word in wordExtractor.GetWords(arg))
                {
                    Console.WriteLine("\"" + word + "\"");
                }
            }
        }
    }
}
