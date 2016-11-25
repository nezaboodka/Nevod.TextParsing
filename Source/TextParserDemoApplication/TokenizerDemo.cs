using System;
using TextParser;

namespace TokenizerDemoApplication
{
    class TokenizerDemo
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
