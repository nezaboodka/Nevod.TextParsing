using System;
using Sharik.Text;

namespace TextParser
{
    public class Parser
    {
        private readonly WordBreakerState fWordBreakerState;
        private readonly ParserState fParserState;
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
            fWordBreakerState = new WordBreakerState(text);
            fParserState = new ParserState();
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
                fParserState.AddCharacter(fText[fCurrentPosition]);
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
                TokenKind = fParserState.TokenKind,
                XhtmlIndex = 0,
                StringPosition = fTokenStart,
                StringLength = fCurrentPosition - fTokenStart + 1
            };
            fParsedText.AddToken(token);
            fTokenStart = fCurrentPosition + 1;
            fParserState.Reset();
        }
    }
}
