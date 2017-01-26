using System;
using TextParser.Common.WordBreak;
using TextParser.PlainText;
using TextParser.Xhtml;

namespace TextParser.Common
{
    public abstract class Parser : IDisposable
    {
        private readonly TokenClassifier fTokenClassifier = new TokenClassifier();                
        private readonly WordBreaker fWordBreaker = new WordBreaker();
        private int fTokenStartPosition;
        private int fTokenStartXhtmlIndex;
        private int fCurrentTokenLength;
        
        protected int fXhtmlIndex;
        protected readonly ParsedText fParsedText = new ParsedText();        
        protected int ProcessingXhtmlIndex => CharacterBuffer.CurrentCharacterInfo.XhtmlIndex;
        protected int ProcessingCharacterIndex => CharacterBuffer.CurrentCharacterInfo.StringPosition;

        internal CharacterBuffer CharacterBuffer { get; } = new CharacterBuffer();

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
            fTokenStartXhtmlIndex = CharacterBuffer.NextCharacterInfo.XhtmlIndex;
            ProcessTags();
            while (NextCharacter())
            {
                fCurrentTokenLength++;
                fTokenClassifier.AddCharacter(CharacterBuffer.CurrentCharacterInfo.Character);
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

        protected virtual bool NextCharacter()
        {
            bool hasNext;
            if (CharacterBuffer.NextCharacter())
            {
                hasNext = true;
            }
            else
            {
                string newBuffer;
                hasNext = FillBuffer(out newBuffer);
                if (hasNext && newBuffer.Length > 0)
                {
                    CharacterBuffer.SetBuffer(newBuffer, fXhtmlIndex);
                }
            }

            if (hasNext)
            {
                char c = CharacterBuffer.NextOfNextCharacterInfo.Character;
                WordBreak.WordBreak wordBreak = WordBreakTable.GetCharacterWordBreak(c);
                fWordBreaker.AddWordBreak(wordBreak);
            }
            else if (HasCharacters())
            {
                MoveNext();
                hasNext = true;
            }
            return hasNext;
        }

        protected abstract bool FillBuffer(out string buffer);

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
            fTokenStartPosition = CharacterBuffer.NextCharacterInfo.StringPosition;
            fTokenStartXhtmlIndex = CharacterBuffer.NextCharacterInfo.XhtmlIndex;
            fCurrentTokenLength = 0;
            fTokenClassifier.Reset();
        }

        protected virtual bool IsBreak()
        {
            return fWordBreaker.IsBreak();
        }

        //private void AddCharacter(char c)
        //{
        //    WordBreak.WordBreak wordBreak = WordBreakTable.GetCharacterWordBreak(c);
        //    fWordBreaker.AddWordBreak(wordBreak);
        //}

        private void MoveNext()
        {
            CharacterBuffer.MoveNext();
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
