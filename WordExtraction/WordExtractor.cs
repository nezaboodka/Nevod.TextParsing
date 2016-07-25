using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordExtraction
{
    class WordExtractor
    {
        public static IEnumerable<string> GetWords(string text)
        {
            yield return "word";
        }
    }
}
