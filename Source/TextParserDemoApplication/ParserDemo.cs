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
                foreach (Token token in Parser.GetTokensFromPlainText(arg))
                {
                    Console.WriteLine("\"" + token.Text + "\"");
                }
            }
        }
    }
}
