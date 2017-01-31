using System;
using TextParser.Common.WordBreaking;
using TextParser.PlainText;
using TextParser.Xhtml;

namespace TextParser.Common.Contract
{
    public abstract class Parser : IDisposable
    {
        protected int fTokenStartPosition;        
        protected readonly ParsedText fParsedText = new ParsedText();
        internal readonly TokenClassifier fTokenClassifier = new TokenClassifier();
        internal readonly WordBreaker fWordBreaker = new WordBreaker();

        protected int CurrentTokenIndex => fParsedText.Tokens.Count - 1;

        // Static public

        public static ParsedText ParsePlainText(string plainText)
        {
            return ParseText(new PlainTextParserFactory(), plainText);
        }

        public static ParsedText ParseXhtmlText(string xhtmlText)
        {
            return ParseText(new XhtmlParserFactory(), xhtmlText);
        }

        // Public

        public abstract void Dispose();

        // Internal

        protected abstract ParsedText Parse();

        // Static internal

        private static ParsedText ParseText(IParserFactory parserFactory, string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            ParsedText result;
            using (var parser = parserFactory.CreateParser(text))
            {
                result = parser.Parse();
            }
            return result;
        }
    }
}
