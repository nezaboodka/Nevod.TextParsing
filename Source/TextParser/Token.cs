namespace TextParser
{
    public enum TokenKind
    {
        Alphabetic,
        Alphanumeric,
        Numeric,
        Symbol,
        WhiteSpace,
        Empty
    }

    public struct Token
    {
        public int XhtmlIndex;
        public int StringPosition;
        public int StringLength;
        public TokenKind TokenKind;
    }
}