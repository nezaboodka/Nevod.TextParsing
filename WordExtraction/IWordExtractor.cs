using System.Collections.Generic;

namespace WordExtraction
{
    interface IWordExtractor
    {
        IEnumerable<string> GetWords(string text);
    }
}
