using System;

namespace TextParser
{
    public abstract class Parser : IDisposable
    {
        protected static readonly TokenReference StartToken = new TokenReference
        {
            StringPosition = 0,
            StringLength = 0,
            TokenKind = TokenKind.Start,
            XhtmlIndex = 0
        };

        protected TokenReference EndToken => new TokenReference
        {
            StringPosition = 0,
            StringLength = 0,
            TokenKind = TokenKind.End,
            XhtmlIndex = fParsedText.XhtmlElements.Count - 1
        };

        protected int fTokenStartPosition;
        protected readonly ParsedText fParsedText = new ParsedText();
        internal readonly TokenClassifier fTokenClassifier = new TokenClassifier();
        internal readonly WordBreaker fWordBreaker = new WordBreaker();

        protected int CurrentTokenIndex => fParsedText.PlainTextTokens.Count - 1;

        // Public

        public abstract void Dispose();

        public abstract ParsedText Parse();
    }
}
