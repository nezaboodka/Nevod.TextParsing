namespace TextParser
{
    internal interface IParserFactory
    {
        Parser CreateParser(string text);
    }
}