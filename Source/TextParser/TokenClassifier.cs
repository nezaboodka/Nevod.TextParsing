using System;

namespace TextParser
{
    internal class TokenClassifier
    {
        internal TokenKind TokenKind { get; private set; }

        internal TokenClassifier()
        {
            TokenKind = TokenKind.Empty;
        }

        internal void Reset()
        {
            TokenKind = TokenKind.Empty;
        }

        internal void AddCharacter(char c)
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
            if (!char.IsWhiteSpace(c))
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
            } else if (char.IsWhiteSpace(c))
            {
                TokenKind = TokenKind.WhiteSpace;
            }
            else
            {
                TokenKind = TokenKind.Symbol;
            }            
        }
    }
}