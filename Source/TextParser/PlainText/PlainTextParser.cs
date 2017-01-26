using TextParser.Common;
using TextParser.Common.WordBreak;

namespace TextParser.PlainText
{
    internal class PlainTextParser : Parser
    {
        private string fText;
        private bool fHasNext;

        // Public

        public PlainTextParser(string text)
        {
            fXhtmlIndex = 0;      
            fParsedText.AddPlainTextElement(text);
            fText = text;
            fHasNext = true;
        }

        public override void Dispose() { }

        // Internal

        protected override bool FillBuffer(out string buffer)
        {
            bool result;
            buffer = default(string);
            if (fHasNext && fText.Length > 0)
            {
                buffer = fText;
                fHasNext = false;
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