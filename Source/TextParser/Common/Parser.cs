using System;
using TextParser.Common.WordBreak;
using TextParser.PlainText;
using TextParser.Xhtml;

namespace TextParser.Common
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
                if (IsBreak())
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
                AddCharacter(c);                            
            }
            else if (HasCharacters())
            {
                MoveNext();
                result = true;
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

        protected virtual bool IsBreak()
        {
            return fWordBreaker.IsBreak();
        }

        private void AddCharacter(char c)
        {
            fCharacterBuffer.AddCharacter(new CharacterInfo(c, fXhtmlIndex, fCurrentPosition));
            WordBreak.WordBreak wordBreak = WordBreakTable.GetCharacterWordBreak(c);
            fWordBreaker.AddWordBreak(wordBreak);
        }

        private void MoveNext()
        {
            fCharacterBuffer.NextCharacter();
            fWordBreaker.NextWordBreak();
        }

        private bool HasCharacters()
        {
            return !fWordBreaker.IsEmptyBuffer();
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
