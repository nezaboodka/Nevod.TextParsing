using System;

namespace TextParser
{
    public abstract class Parser : IDisposable
    {
        private readonly TokenClassifier fTokenClassifier = new TokenClassifier();                
        private readonly CharacterBuffer fCharacterBuffer = new CharacterBuffer();
        private readonly WordBreaker fWordBreaker = new WordBreaker();
        private int fTokenStartPosition;
        private int fTokenStartXhtmlIndex;

        protected int fXhtmlIndex;
        protected int fCurrentPosition;        
        protected readonly ParsedText fParsedText = new ParsedText();
        
        // Public static

        public static ParsedText GetTokensFromPlainText(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            Parser parser = new PlainTextParser(text);
            return parser.Parse();
        }

        public static ParsedText GetTokensFromXhtml(string xhtmlText)
        {
            throw new NotImplementedException();
        }

        // Public

        public abstract void Dispose();

        // Internals

        private ParsedText Parse()
        {
            InitializeBuffer();
            while (NextCharacter())
            {                
                fTokenClassifier.AddCharacter(fCharacterBuffer.CurrentCharacterInfo.Character);
                if (fWordBreaker.IsBreak())
                {
                    SaveToken();
                }                
            }
            return fParsedText;
        }

        private void InitializeBuffer()
        {
            NextCharacter();
            NextCharacter();
        }

        private bool NextCharacter()
        {
            char c;
            bool result = Read(out c);
            if (result)
            {
                fCharacterBuffer.AddCharacter(new CharacterInfo(c, fXhtmlIndex, fCurrentPosition));
                WordBreak wordBreak = WordBreakTable.GetCharacterWordBreak(c);
                fWordBreaker.AddWordBreak(wordBreak);                
            }
            else
            {
                if (!fWordBreaker.IsEmptyBuffer())
                {
                    fCharacterBuffer.NextCharacter();
                    fWordBreaker.NextWordBreak();
                    result = true;
                }
            }            
            return result;
        }

        protected abstract bool Read(out char c);

        private void SaveToken()
        {
            int currentCharacterPosition = fCharacterBuffer.CurrentCharacterInfo.StringPosition;
            var token = new Token
            {
                TokenKind = fTokenClassifier.TokenKind,
                XhtmlIndex = fTokenStartXhtmlIndex,
                StringPosition = fTokenStartPosition,
                StringLength = currentCharacterPosition - fTokenStartPosition + 1
            };
            fParsedText.AddToken(token);
            fTokenStartPosition = fCharacterBuffer.NextCharacterInfo.StringPosition;
            fTokenStartXhtmlIndex = fCharacterBuffer.NextCharacterInfo.XhtmlIndex;
            fTokenClassifier.Reset();
        }
    }
}
