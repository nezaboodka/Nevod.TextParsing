using System.Collections.Generic;

namespace WordExtraction
{
    public class StandardWordExtractor : IWordExtractor
    {
        private IScanWindow ScanWindow { get; set; }

        public StandardWordExtractor(IScanWindow scanWindow)
        {
            ScanWindow = scanWindow;
        }

        public StandardWordExtractor()
        {
            ScanWindow = new StandardScanWindow();
        }

        public IEnumerable<string> GetWords(string text)
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

            for (int i = 0; i < text.Length; i++)
            {
                if (i == text.Length - 1)
                    ScanWindow.MoveWindow();
                else
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
            if (isWord)
            {
                yield return text.Substring(startPosition, text.Length - startPosition);
            }
        }
    }
}
