using System.Collections.Generic;
using System.Linq;

namespace WordExtraction
{
    public class WordExtractor
    {
        private ScanWindow ScanWindow { get; set; }

        public WordExtractor(ScanWindow scanWindow)
        {
            ScanWindow = scanWindow;
        }

        public WordExtractor()
        {
            ScanWindow = new ScanWindow();
        }

        public virtual IEnumerable<string> GetWords(string text)
        {
            if (string.IsNullOrEmpty(text))
                yield break;

            if (text.Length == 1)
            {
                yield return text;
                yield break;
            }

            ScanWindow.AddSymbol(text[0]);
            int startPosition = 0;
            bool isWord = false;

            for (int i = 0; i < text.Length - 1; i++)
            {
                ScanWindow.AddSymbol(text[i + 1]);

                if (ScanWindow.CheckForBreak())
                {
                    if (isWord)
                        yield return text.Substring(startPosition, i - startPosition);
                    startPosition = i;
                    isWord = false;
                }

                if (!isWord && char.IsLetterOrDigit(text[i]))
                    isWord = true;
            }

            ScanWindow.MoveWindow();

            if (ScanWindow.CheckForBreak())
            {
                if (isWord)
                    yield return text.Substring(startPosition, text.Length - startPosition - 1);
                startPosition = text.Length - 1;
            }

            if (char.IsLetterOrDigit(text.Last()))
            {
                yield return text.Substring(startPosition);
            }
        }
    }
}
