using System.Collections.Generic;
using TextParser.Common;
using TextParser.Common.Contract;

namespace TextParser.Tests
{
    internal static class ParsedTextTestHelper
    {
        public static void SetXhtml(this ParsedText parsedText, IList<string> xhtml, ISet<int> plainTextElements)
        {
            for (int i = 0; i < xhtml.Count; i++)
            {
                if (plainTextElements.Contains(i))
                {
                    parsedText.AddPlainTextElement(xhtml[i]);
                }
                else
                {
                    parsedText.AddXhtmlElement(xhtml[i]);
                }
            }
        }
    }
}