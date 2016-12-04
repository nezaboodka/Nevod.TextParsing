using System.Collections.Generic;

namespace TextParser.Tests
{
    internal static class ParsedTextTestHelper
    {
        public static void SetXhtml(this ParsedText parsedText, IList<string> xhtml, ISet<int> plainTextElements)
        {
            for (int i = 0; i < xhtml.Count; i++)
            {
                bool isPlainText = plainTextElements.Contains(i);
                parsedText.AddXhtmlElement(xhtml[i], isPlainText);
            }
        }
    }
}