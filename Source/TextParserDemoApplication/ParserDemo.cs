using System;
using TextParser;

namespace TextParserDemoApplication
{
    class ParserDemo
    {
        static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                ParsedText parsedText = Parser.ParsePlainText(arg);
                foreach (Token token in parsedText.Tokens)
                {
                    Console.WriteLine("\"" + parsedText.GetPlainText(token) + "\"");
                }
            }
        }
    }
}
