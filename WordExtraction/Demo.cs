using System;

namespace WordExtraction
{
    class Demo
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
