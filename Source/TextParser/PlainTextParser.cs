using System;

namespace TextParser
{
    internal class PlainTextParser : Parser
    {
        private readonly string fText;        

        // Public

        public PlainTextParser(string text)
        {
            fText = text;
            fCurrentPosition = -1;
            fXhtmlIndex = 0;      
            fParsedText.AddPlainTextElement(text);
        }

        public override void Dispose() { }

        // Internals

        protected override bool Read(out char c)
        {
            bool result;
            c = default(char);
            if (fCurrentPosition < fText.Length - 1)
            {
                fCurrentPosition++;
                c = fText[fCurrentPosition];
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}