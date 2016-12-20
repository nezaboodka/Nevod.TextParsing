using TextParser.Common.WordBreak;

namespace TextParser.Common
{
    internal class TokenClassifier
    {
        private const char CR = '\r';
        private const char LF = '\n';

        // Public

        public TokenKind TokenKind { get; private set; }

        public TokenClassifier()
        {
            TokenKind = TokenKind.Empty;
        }

        public void Reset()
        {
            TokenKind = TokenKind.Empty;
        }

        public void AddCharacter(char c)
        {
            switch (TokenKind)
            {
                case TokenKind.Alphabetic:
                    ProcessAlphabetic(c);
                    break;
                case TokenKind.Alphanumeric:
                    ProcessAlphanumeric(c);
                    break;
                case TokenKind.Numeric:
                    ProcessNumeric(c);
                    break;
                case TokenKind.WhiteSpace:
                    ProcessWhiteSpace(c);
                    break;
                case TokenKind.Empty:
                    ProcessEmpty(c);
                    break;
            }
        }

        // Internals

        private void ProcessAlphabetic(char c)
        {
            if (char.IsDigit(c))
            {
                TokenKind = TokenKind.Alphanumeric;
            }
            else if (!char.IsLetter(c))
            {
                TokenKind = TokenKind.Symbol;
            }
        }

        private void ProcessAlphanumeric(char c)
        {
            if (!char.IsLetterOrDigit(c))
            {
                TokenKind = TokenKind.Symbol;
            }

        }

        private void ProcessNumeric(char c)
        {
            if (char.IsLetter(c))
            {
                TokenKind = TokenKind.Alphanumeric;
            }
            else if (!char.IsDigit(c))
            {
                TokenKind = TokenKind.Symbol;
            }
        }

        private void ProcessWhiteSpace(char c)
        {
            if (WordBreakTable.GetCharacterWordBreak(c) != WordBreak.WordBreak.Whitespace)
            {
                TokenKind = TokenKind.Symbol;                
            }
            
        }

        private void ProcessEmpty(char c)
        {
            if (char.IsDigit(c))
            {
                TokenKind = TokenKind.Numeric;                
            } else if (char.IsLetter(c))
            {
                TokenKind = TokenKind.Alphabetic;
            } else if (WordBreakTable.GetCharacterWordBreak(c) == WordBreak.WordBreak.Whitespace)
            {
                TokenKind = TokenKind.WhiteSpace;
            } else if (IsLineSeparator(c))
            {
                TokenKind = TokenKind.LineFeed;                
            }
            else
            {
                TokenKind = TokenKind.Symbol;
            }            
        }

        // Static internals

        private static bool IsLineSeparator(char c)
        {
            return (c == CR) || (c == LF);
        }
    }
}