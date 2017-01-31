using TextParser.Common;
using TextParser.Common.Contract;
using TextParser.Common.WordBreaking;
using TextParser.PlainText.Tagging;

namespace TextParser.PlainText
{
    internal class PlainTextParser : Parser
    {
        private const int LookAheadSize = 2;
        private readonly string fText;
        private readonly PlainTextParapraphsTagger fPlainTextParapraphsTagger;
        private int fLookAheadPosition = -1;

        private int CurrentPosition => fLookAheadPosition - LookAheadSize;

        // Public

        public PlainTextParser(string text)
        {
            fParsedText.AddPlainTextElement(text);
            fText = text;
            fPlainTextParapraphsTagger = new PlainTextParapraphsTagger(fParsedText);
        }

        public override void Dispose() { }

        // Internal

        protected override ParsedText Parse()
        {
            InitializeLookahead();
            while (NextCharacter())
            {
                fTokenClassifier.AddCharacter(fText[CurrentPosition]);
                if (fWordBreaker.IsBreak())
                {
                    SaveToken();
                    ProcessTags();
                }
            }
            return fParsedText;
        }

        private void InitializeLookahead()
        {
            NextCharacter();
            NextCharacter();
        }

        private void SaveToken()
        {
            var token = new Token
            {
                TokenKind = fTokenClassifier.TokenKind,
                XhtmlIndex = 0,
                StringPosition = fTokenStartPosition,
                StringLength = CurrentPosition - fTokenStartPosition + 1
            };
            fParsedText.AddToken(token);
            fTokenStartPosition = CurrentPosition + 1;
            fTokenClassifier.Reset();
        }

        private bool NextCharacter()
        {
            bool result;
            if (CurrentPosition < fText.Length - 1)
            {
                fLookAheadPosition++;
                result = true;
            }
            else
            {
                result = false;
            }
            if (fLookAheadPosition < fText.Length)
            {
                WordBreak wordBreak = WordBreakTable.GetCharacterWordBreak(fText[fLookAheadPosition]);
                fWordBreaker.AddWordBreak(wordBreak);
            }
            else
            {
                fWordBreaker.NextWordBreak();
            }
            return result;
        }

        private void ProcessTags()
        {
            if (CurrentTokenIndex >= 0)
            {
                if (CurrentPosition < fText.Length - 1)
                {
                    TokenKind lastTokenKind = fParsedText.Tokens[CurrentTokenIndex].TokenKind;
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