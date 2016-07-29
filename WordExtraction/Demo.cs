using System;

namespace WordExtraction
{
    class Demo
    {
        private const string UNICODE_DEMO = "The quick(\"brown\") fox can't jump 32.3 feet, right. But I can! С кириллицей все в порядке.";

        static void Main(string[] args)
        {
            WordExtractor wordExtractor = new WordExtractor();
            foreach (var word in wordExtractor.GetWords(UNICODE_DEMO))
            {
                Console.WriteLine("\"" + word + "\"");
            }
        }
    }
}
