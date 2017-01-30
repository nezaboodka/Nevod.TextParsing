using TextParser.Common;
using TextParser.Common.WordBreak;

namespace TextParser.PlainText
{
    internal class PlainTextParser : Parser
    {
        private readonly string fText;
        private bool fHasNext;
        private readonly PlainTextParapraphsTagger fPlainTextParapraphsTagger;        

        // Public

        public PlainTextParser(string text)
        {
            fXhtmlIndex = 0;      
            fParsedText.AddPlainTextElement(text);
            fText = text;
            fHasNext = true;
            fPlainTextParapraphsTagger = new PlainTextParapraphsTagger(fParsedText);
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

        protected override void ProcessTags()
        {
            if (fParsedText.Tokens.Count > 0)
            {
                if (CharacterBuffer.CurrentCharacterInfo.StringPosition < fText.Length - 1)
                {
                    TokenKind lastTokenKind = fParsedText.Tokens[fParsedText.Tokens.Count - 1].TokenKind;
                    fPlainTextParapraphsTagger.ProcessToken(lastTokenKind);
                }
                else
                {
                    fPlainTextParapraphsTagger.ProcessEndOfText();
                }
                
            }
        }        
    }
}