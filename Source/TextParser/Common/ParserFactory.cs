namespace TextParser.Common
{
    internal interface IParserFactory
    {
        Parser CreateParser(string text);
    }
}