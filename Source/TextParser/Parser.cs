using System;

namespace TextParser
{
    public abstract class Parser : IDisposable
    {
        protected int fTokenStartPosition;        
        protected readonly ParsedText fParsedText = new ParsedText();
        internal readonly TokenClassifier fTokenClassifier = new TokenClassifier();
        internal readonly WordBreaker fWordBreaker = new WordBreaker();

        protected int CurrentTokenIndex => fParsedText.TextTokens.Count - 1;

        // Public

        public abstract void Dispose();

        public abstract ParsedText Parse();
    }
}
