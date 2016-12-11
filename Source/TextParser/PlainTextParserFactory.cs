namespace TextParser
{
    internal class PlainTextParserFactory : IParserFactory
    {
        public Parser CreateParser(string text)
        {
            return new PlainTextParser(text);
        }
    }    
}