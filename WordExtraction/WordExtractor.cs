using System.Collections.Generic;

namespace WordExtraction
{
    public class WordExtractor : IWordExtractor
    {
        public IEnumerable<string> GetWords(string text)
        {
            if (string.IsNullOrEmpty(text))
                yield break;

            if (text.Length == 1)
            {
                yield return text;
                yield break;
            }

            var scanWindow = new ScanWindow(text[0]);
            int startPosition = 0;
            bool isWord = false;

            for (int i = 0; i < text.Length; i++)
            {
                if (i == text.Length - 1)
                    scanWindow.AddEmpty();
                else
                    scanWindow.AddSymbol(text[i + 1]);

                if (scanWindow.CheckForBreak())
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
