using System;
using TextParser;

namespace TokenizerDemoApplication
{
    class TokenizerDemo
    {
        static void Main(string[] args)
        {
            Tokenizer tokenizer = new Tokenizer();
            foreach (string arg in args)
            {
                foreach (Token token in tokenizer.GetTokens(arg))
                {
                    Console.WriteLine("\"" + token.Text + "\"");
                }
            }
        }
    }
}
