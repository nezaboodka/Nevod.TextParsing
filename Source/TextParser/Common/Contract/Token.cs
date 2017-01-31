namespace TextParser.Common.Contract
{
    public enum TokenKind
    {
        Alphabetic,
        Alphanumeric,
        LineFeed,
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