using TextParser.Common;
using TextParser.Common.Contract;

namespace TextParser.PlainText
{
    internal class PlainTextParserFactory : IParserFactory
    {
        public Parser CreateParser(string text)
        {
            return new PlainTextParser(text);
        }
    }    
}