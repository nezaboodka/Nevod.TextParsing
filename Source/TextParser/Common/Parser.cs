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
        protected int fCharacterIndex;        
        protected readonly ParsedText fParsedText = new ParsedText();
        protected string fBuffer;

        protected int ProcessingXhtmlIndex => fCharacterBuffer.CurrentCharacterInfo.XhtmlIndex;
        protected int ProcessingCharacterIndex => fCharacterBuffer.CurrentCharacterInfo.StringPosition;
        
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

        // Internal

        protected virtual ParsedText Parse()
        {
            InitializeBuffer();
            fTokenStartXhtmlIndex = fCharacterBuffer.NextCharacterInfo.XhtmlIndex;
            ProcessTags();
            while (NextCharacter())
            {
                fCurrentTokenLength++;
                fTokenClassifier.AddCharacter(fCharacterBuffer.CurrentCharacterInfo.Character);
                if (IsBreak())
                {
                    SaveToken();
                    ProcessTags();
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
            bool hasNext;
            char c;
            if ((fBuffer != null) && (fCharacterIndex < fBuffer.Length - 1))
            {
                fCharacterIndex++;
                hasNext = true;
            }
            else
            {
                hasNext = FillBuffer();
                if (hasNext)
                {
                    fCharacterIndex = 0;
                }
            }
            
            if (hasNext)
            {
                c = fBuffer[fCharacterIndex];
                AddCharacter(c);                            
            }
            else if (HasCharacters())
            {
                MoveNext();
                hasNext = true;
            }                    
            return hasNext;
        }

        protected abstract bool FillBuffer();

        protected abstract void ProcessTags();

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
            fCharacterBuffer.AddCharacter(new CharacterInfo(c, fXhtmlIndex, fCharacterIndex));
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

        // Static internal

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
