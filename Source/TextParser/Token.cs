using System;
using Sharik.Text;

namespace TextParser
{
    public struct Token
    {
        public const byte WhiteSpace = 0;
        public const byte Alphabetic = 1;
        public const byte Alphanumeric = 2;
        public const byte Numeric = 3;
        public const byte Symbol = 4;

        public Slice Text;
        public byte TokenKind;

        public Token(Slice text, byte tokenKind)
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