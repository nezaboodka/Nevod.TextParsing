namespace TextParser
{
    internal class XhtmlParserFactory : IParserFactory
    {
        public Parser CreateParser(string text)
        {
            return new XhtmlParser(text);
        }
    }
}