using System.Collections.Generic;
using System.Linq;

namespace WordExtraction
{
    public class StandardWordExtractor : WordExtractor
    {
        private ScanWindow ScanWindow { get; set; }

        public StandardWordExtractor(ScanWindow scanWindow)
        {
            ScanWindow = scanWindow;
        }

        public StandardWordExtractor()
        {
            ScanWindow = new StandardScanWindow();
        }

        public override IEnumerable<string> GetWords(string text)
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
