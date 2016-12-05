using System;
using Sharik.Text;

namespace TextParser
{
    public class Parser
    {
        private readonly WordBreaker fWordBreakerState;
        private readonly TokenClassifier fTokenClassifier;
        private readonly string fText;
        private int fCurrentPosition;
        private int fTokenStart;
        private readonly ParsedText fParsedText;

        public static ParsedText GetTokensFromPlainText(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            var parser = new Parser(text);
            return parser.GetTokensFromPlainText();
        }

        // Internals

        private Parser(string text)
        {
            fText = text;
            fWordBreakerState = new WordBreaker(text);
            fTokenClassifier = new TokenClassifier();
            fParsedText = new ParsedText();
            fParsedText.AddXhtmlElement(text, true);
            fCurrentPosition = -1;
            fTokenStart = 0;
        }
        
        protected virtual ParsedText GetTokensFromPlainText()
        {
            while (NextCharacter())
            {
                if (IsBreak())
                {
                    NextToken();
                }
            }                

            return fParsedText;
        }

        protected virtual bool NextCharacter()
        {
            bool result = false;
            fCurrentPosition++;
            if (fCurrentPosition < fText.Length)
            {
                fWordBreakerState.NextCharacter();
                fTokenClassifier.AddCharacter(fText[fCurrentPosition]);
                result = true;
            }
            return result;
        }

        protected virtual bool IsBreak()
        {
            return fWordBreakerState.IsBreak();
        }

        protected virtual void NextToken()
        {
            var token = new Token
            {
                TokenKind = fTokenClassifier.TokenKind,
                XhtmlIndex = 0,
                StringPosition = fTokenStart,
                StringLength = fCurrentPosition - fTokenStart + 1
            };
            fParsedText.AddToken(token);
            fTokenStart = fCurrentPosition + 1;
            fTokenClassifier.Reset();
        }
    }
}
