using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordExtraction
{
    class Demo
    {
        private const string UNICODE_DEMO = "The quick(\"brown\") fox can't jump 32.3 feet, right?";

        static void Main(string[] args)
        {
            foreach (var word in WordExtractor.GetWords(UNICODE_DEMO))
            {
                Console.WriteLine(word);
            }
        }
    }
}
