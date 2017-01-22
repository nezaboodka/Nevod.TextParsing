using TextParser.Common;

namespace TextParser.PlainText
{
    internal class PlainTextParser : Parser
    {
        private readonly string fText;        

        // Public

        public PlainTextParser(string text)
        {
            fText = text;
            fCharacterIndex = -1;
            fXhtmlIndex = 0;      
            fParsedText.AddPlainTextElement(text);
        }

        public override void Dispose() { }

        // Internals

        protected override bool Read(out char c)
        {
            bool result;
            c = default(char);
            if (fCharacterIndex < fText.Length - 1)
            {
                fCharacterIndex++;
                c = fText[fCharacterIndex];
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        protected override void ProcessTags() { }
    }
}