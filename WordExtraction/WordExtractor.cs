using System.Collections.Generic;

namespace WordExtraction
{
    class WordExtractor
    {
        class ScanWindow
        {
            public WordBreakPropertyType? Ahead { get; private set; }

            public WordBreakPropertyType? Current { get; private set; }

            public WordBreakPropertyType? Behind { get; private set; }

            public WordBreakPropertyType? BehindOfBehind { get; private set; }

            public void Add(WordBreakPropertyType? nextSymbolType)
            {
                BehindOfBehind = Behind;
                Behind = Current;
                Current = Ahead;
                Ahead = nextSymbolType;
            }

            public ScanWindow(WordBreakPropertyType? firstSymbolType, WordBreakPropertyType? nexSymbolType)
            {
                Add(firstSymbolType);
                Add(nexSymbolType);
            }
        }

        private const int CACHE_SIZE = 3;
        public static IEnumerable<string> GetWords(string text)
        {
            if (string.IsNullOrEmpty(text))
                yield break;

            WordBreakPropertyType[] typesCache = new WordBreakPropertyType[CACHE_SIZE];
            int i = 0;
            foreach (char c in text)
            {

            }
            yield return "word";
        }
    }
}
