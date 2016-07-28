using System.Collections.Generic;

namespace WordExtraction
{
    class WordExtractor
    {
        class ScanWindow
        {
            public SymbolType? Ahead { get; private set; }

            public SymbolType? Current { get; private set; }

            public SymbolType? Behind { get; private set; }

            public SymbolType? BehindOfBehind { get; private set; }

            public void Add(SymbolType? nextSymbolType)
            {
                BehindOfBehind = Behind;
                Behind = Current;
                Current = Ahead;
                Ahead = nextSymbolType;
            }

            public ScanWindow(SymbolType? firstSymbolType, SymbolType? nexSymbolType)
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

            if (text.Length == 1)
            {
                yield return text;
                yield break;
            }
                

            ScanWindow scanWindow = new ScanWindow(SymbolTable.GetSymbolType(text[0]), )
            int i = 0;
            foreach (char c in text)
            {

            }
            yield return "word";
        }
    }
}
