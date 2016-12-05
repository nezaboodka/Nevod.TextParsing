using System;

namespace TextParser
{
    internal class PlainTextParser : Parser
    {
        private readonly WordBreaker fWordBreakerState;
        private readonly TokenClassifier fTokenClassifier;
        private readonly string fText;
        private int fCurrentPosition;
        private int fTokenStart;
        private readonly ParsedText fParsedText;

        internal PlainTextParser(string text)
        {
            fText = text;
            fWordBreakerState = new WordBreaker(text);
            fTokenClassifier = new TokenClassifier();
            fParsedText = new ParsedText();
            fParsedText.AddXhtmlElement(text, true);
            fCurrentPosition = -1;
            fTokenStart = 0;
        }

        protected override ParsedText Parse()
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

        private bool NextCharacter()
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

        private bool IsBreak()
        {
            return fWordBreakerState.IsBreak();
        }

        private void NextToken()
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