using System;

namespace WordExtraction
{
    class Demo
    {
        private const string UNICODE_DEMO = "The quick(\"brown\") fox can't jump 32.3 feet, right?";

        static void Main(string[] args)
        {
            WordExtractor wordExtractor = new WordExtractor();
            foreach (var word in wordExtractor.GetWords(UNICODE_DEMO))
            {
                Console.WriteLine(word);
            }
            //Console.WriteLine(SymbolTable.GetSymbolType(' '));
            //Console.WriteLine(SymbolTable.GetSymbolType('e'));
            //Console.WriteLine(SymbolTable.GetSymbolType('z'));
            //Console.WriteLine(SymbolTable.GetSymbolType('g'));
        }
    }
}
