using System;

namespace TextParser
{
    public class PlainTextParser : Parser
    {
        private const int LookAheadSize = 2;
        private readonly string fText;
        private readonly PlainTextTagger fPlainTextParapraphsTagger;
        private int fLookAheadPosition = -1;

        private int CurrentPosition => fLookAheadPosition - LookAheadSize;

        // Public

        public static ParsedText Parse(string plainText)
        {
            ParsedText result;
            using (var parser = new PlainTextParser(plainText))
                result = parser.Parse();
            return result;
        }

        public PlainTextParser(string plainText)
        {
            if (plainText != null)
            {
                fParsedText.AddPlainTextElement(plainText);
                fText = plainText;
                fPlainTextParapraphsTagger = new PlainTextTagger(fParsedText);
            }
            else
                throw new ArgumentNullException(nameof(plainText));
        }

        public override void Dispose()
        {
        }

        public override ParsedText Parse()
        {
            fParsedText.AddToken(StartToken);
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
            fParsedText.AddToken(EndToken);
            return fParsedText;
        }

        // Internal

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
                result = false;
            if (fLookAheadPosition < fText.Length)
            {
                WordBreak wordBreak = WordBreakTable.GetCharacterWordBreak(fText[fLookAheadPosition]);
                fWordBreaker.AddWordBreak(wordBreak);
            }
            else
                fWordBreaker.NextWordBreak();
            return result;
        }

        private void ProcessTags()
        {
            if (CurrentTokenIndex >= 0)
            {
                if (CurrentPosition < fText.Length - 1)
                {
                    TokenKind lastTokenKind = fParsedText.TextTokens[CurrentTokenIndex].TokenKind;
                    fPlainTextParapraphsTagger.ProcessToken(lastTokenKind);
                }
                else
                    fPlainTextParapraphsTagger.ProcessEndOfText();
            }
        }
    }
}
