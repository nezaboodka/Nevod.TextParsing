using TextParser.Common;

namespace TextParser.PlainText
{
    internal class PlainTextParser : Parser
    {
        public PlainTextParser(string text)
        {
            fCharacterIndex = -1;
            fXhtmlIndex = 0;      
            fParsedText.AddPlainTextElement(text);
            fBuffer = text;
        }

        public override void Dispose() { }

        // Internal

        protected override bool FillBuffer()
        {
            return false;
        }

        protected override void ProcessTags() { }
    }
}