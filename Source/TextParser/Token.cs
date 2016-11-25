using System;
using Sharik.Text;

namespace TextParser
{
    public enum TokenKind : byte
    {
        WhiteSpace,
        Alphabetic,
        Alphanumeric,
        Numeric,
        Symbol
    }

    public struct Token
    {
        public Slice Text;
        public TokenKind TokenKind;

        public Token(Slice text, TokenKind tokenKind)
        {
            Text = text;
            TokenKind = tokenKind;
        }

        public override bool Equals(object obj)
        {
            bool result;
            if (obj == null)
            {
                result = false;
            }
            else
            {                
                if (obj is Token)
                {
                    Token token = (Token) obj;
                    result = (Text.CompareTo(token.Text) == 0) && (TokenKind == token.TokenKind);
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}