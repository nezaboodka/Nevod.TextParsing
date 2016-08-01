using System.Collections.Generic;

namespace WordExtraction
{
    public interface IWordExtractor
    {
        IEnumerable<string> GetWords(string text);
    }
}
