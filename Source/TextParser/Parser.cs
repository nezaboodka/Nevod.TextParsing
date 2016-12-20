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
        private int fCurrentTokenLength;

        protected int fXhtmlIndex;
        protected int fCurrentPosition;        
        protected readonly ParsedText fParsedText = new ParsedText();
        
        // Static public

        public static ParsedText ParsePlainText(string plainText)
        {
            return ParseText(new PlainTextParserFactory(), plainText);
        }

        public static ParsedText ParseXhtmlText(string xhtmlText)
        {
            return ParseText(new XhtmlParserFactory(), xhtmlText);
        }

        // Public

        public abstract void Dispose();

        // Internals

        private ParsedText Parse()
        {
            InitializeBuffer();
            fTokenStartXhtmlIndex = fCharacterBuffer.NextOfNextCharacterInfo.XhtmlIndex;
            while (NextCharacter())
            {
                fCurrentTokenLength++;
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
            var token = new Token
            {
                TokenKind = fTokenClassifier.TokenKind,
                XhtmlIndex = fTokenStartXhtmlIndex,
                StringPosition = fTokenStartPosition,
                StringLength = fCurrentTokenLength
            };
            fParsedText.AddToken(token);
            fTokenStartPosition = fCharacterBuffer.NextCharacterInfo.StringPosition;
            fTokenStartXhtmlIndex = fCharacterBuffer.NextCharacterInfo.XhtmlIndex;
            fCurrentTokenLength = 0;
            fTokenClassifier.Reset();
        }

        // Static internals

        private static ParsedText ParseText(IParserFactory parserFactory, string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            ParsedText result;
            using (var parser = parserFactory.CreateParser(text))
            {
                result = parser.Parse();
            }
            return result;
        }
    }
}
