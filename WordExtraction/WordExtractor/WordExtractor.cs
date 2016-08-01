using System.Collections.Generic;

namespace WordExtraction
{
    public abstract class WordExtractor
    {
        public abstract IEnumerable<string> GetWords(string text);
    }
}
