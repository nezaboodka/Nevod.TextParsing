using System;

namespace TextParser
{
    public abstract class Parser : IDisposable
    {
        private readonly TokenClassifier fTokenClassifier = new TokenClassifier();                
        private readonly CharacterBuffer fCharacterBuffer = new CharacterBuffer();
        private readonly WordBreakBuffer fWordBreakBuffer = new WordBreakBuffer();
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
                fCharacterBuffer.AddCharacter(new CharacterInfo(c, fXhtmlIndex, fCurrentPosition));
                WordBreak wordBreak = WordBreakTable.GetCharacterWordBreak(c);
                if (!ShoudIgnore(wordBreak)) // WB4.
                {
                    fWordBreakBuffer.AddWordBreak(wordBreak);   
                }
            }
            else
            {
                if (!IsEmptyBuffer())
                {
                    ShiftBuffers();
                    result = true;
                }
            }            
            return result;
        }

        protected abstract bool Read(out char c);

        private bool IsBreak()
        {
            bool result;
            // WB2.
            if (!IsEnoughCharacters() && !IsLastCharacter())
                result = false;
            // WB3.
            else if (IsLineBreak(fWordBreakBuffer.CurrentWordBreak) || IsLineBreak(fWordBreakBuffer.NextWordBreak))
                result = !((fWordBreakBuffer.CurrentWordBreak == WordBreak.CarriageReturn) && (fWordBreakBuffer.NextWordBreak == WordBreak.LineFeed));
            // WB5.
            else if (IsAlphabeticOrHebrewLetter(fWordBreakBuffer.CurrentWordBreak) && IsAlphabeticOrHebrewLetter(fWordBreakBuffer.NextWordBreak))
                result = false;
            // WB6. (without single quotes)
            else if (IsAlphabeticOrHebrewLetter(fWordBreakBuffer.CurrentWordBreak) && ((fWordBreakBuffer.NextWordBreak == WordBreak.MidLetter) ||
                (fWordBreakBuffer.NextWordBreak == WordBreak.MidNumberAndLetter))
                && IsAlphabeticOrHebrewLetter(fWordBreakBuffer.NextOfNextWordBreak))
                result = false;
            // WB7. (without single quotes)
            else if (IsAlphabeticOrHebrewLetter(fWordBreakBuffer.PreviousWordBreak) && ((fWordBreakBuffer.CurrentWordBreak == WordBreak.MidLetter)
                || (fWordBreakBuffer.CurrentWordBreak == WordBreak.MidNumberAndLetter))
                && IsAlphabeticOrHebrewLetter(fWordBreakBuffer.NextWordBreak))
                result = false;
            // WB7a.
            else if ((fWordBreakBuffer.CurrentWordBreak == WordBreak.HebrewLetter) && (fWordBreakBuffer.NextWordBreak == WordBreak.SingleQuote))
                result = false;
            // WB7b.
            else if ((fWordBreakBuffer.CurrentWordBreak == WordBreak.HebrewLetter) && (fWordBreakBuffer.NextWordBreak == WordBreak.DoubleQuote)
                && (fWordBreakBuffer.NextOfNextWordBreak == WordBreak.HebrewLetter))
                result = false;
            // WB7c.
            else if ((fWordBreakBuffer.PreviousWordBreak == WordBreak.HebrewLetter) && (fWordBreakBuffer.CurrentWordBreak == WordBreak.DoubleQuote)
                && (fWordBreakBuffer.NextWordBreak == WordBreak.HebrewLetter))
                result = false;
            // WB8.
            else if ((fWordBreakBuffer.CurrentWordBreak == WordBreak.Numeric) && (fWordBreakBuffer.NextWordBreak == WordBreak.Numeric))
                result = false;
            // WB9.
            else if (IsAlphabeticOrHebrewLetter(fWordBreakBuffer.CurrentWordBreak) && fWordBreakBuffer.NextWordBreak == WordBreak.Numeric)
                result = false;
            // WB10.
            else if ((fWordBreakBuffer.CurrentWordBreak == WordBreak.Numeric) && IsAlphabeticOrHebrewLetter(fWordBreakBuffer.NextWordBreak))
                result = false;
            // WB13.
            else if ((fWordBreakBuffer.CurrentWordBreak == WordBreak.Katakana) && (fWordBreakBuffer.NextWordBreak == WordBreak.Katakana))
                result = false;
            // WB13a.
            else if ((IsAlphabeticOrHebrewLetter(fWordBreakBuffer.CurrentWordBreak) || (fWordBreakBuffer.CurrentWordBreak == WordBreak.Numeric)
                || (fWordBreakBuffer.CurrentWordBreak == WordBreak.Katakana) || (fWordBreakBuffer.CurrentWordBreak == WordBreak.ExtenderForNumbersAndLetters))
                && (fWordBreakBuffer.NextWordBreak == WordBreak.ExtenderForNumbersAndLetters))
                result = false;
            // WB13b.
            else if ((fWordBreakBuffer.CurrentWordBreak == WordBreak.ExtenderForNumbersAndLetters) && (IsAlphabeticOrHebrewLetter(fWordBreakBuffer.NextWordBreak)
                || (fWordBreakBuffer.NextWordBreak == WordBreak.Numeric) || (fWordBreakBuffer.NextWordBreak == WordBreak.Katakana)))
                result = false;
            // custom: do not break between whitespaces
            else if ((fWordBreakBuffer.CurrentWordBreak == WordBreak.Whitespace) && (fWordBreakBuffer.NextWordBreak == WordBreak.Whitespace))
                result = false;
            // WB14.
            else
                result = true;
            return result;
        }

        private void ShiftBuffers()
        {
            fCharacterBuffer.ShiftBuffer();
            fWordBreakBuffer.ShiftBuffer();
        }

        private bool IsEnoughCharacters()
        {
            return (fWordBreakBuffer.NextWordBreak != WordBreak.Empty) && (fWordBreakBuffer.CurrentWordBreak != WordBreak.Empty);
        }

        private bool IsLastCharacter()
        {
            return (fWordBreakBuffer.CurrentWordBreak != WordBreak.Empty) && (fWordBreakBuffer.NextWordBreak == WordBreak.Empty);
        }

        private bool IsEmptyBuffer()
        {
            return (fWordBreakBuffer.CurrentWordBreak == WordBreak.Empty) && (fWordBreakBuffer.NextWordBreak == WordBreak.Empty) && (fWordBreakBuffer.NextOfNextWordBreak == WordBreak.Empty);
        }

        private bool IsFirstCharacter()
        {
            return (fWordBreakBuffer.PreviousWordBreak == WordBreak.Empty) && (fWordBreakBuffer.NextOfNextWordBreak == WordBreak.Empty);
        }

        private bool ShoudIgnore(WordBreak wordBreak)
        {
            return ((wordBreak == WordBreak.Extender) || (wordBreak == WordBreak.Format)) && !IsFirstCharacter();
        }

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
            fTokenStartPosition = currentCharacterPosition + 1;
            fTokenStartXhtmlIndex = fXhtmlIndex;
            fTokenClassifier.Reset();
        }

        // Static internals        

        private static bool IsAlphabeticOrHebrewLetter(WordBreak wordBreak)
        {
            return (wordBreak == WordBreak.AlphabeticLetter) || (wordBreak == WordBreak.HebrewLetter);
        }

        private static bool IsLineBreak(WordBreak wordBreak)
        {
            return (wordBreak == WordBreak.Newline) || (wordBreak == WordBreak.LineFeed) || (wordBreak == WordBreak.CarriageReturn);
        }        
    }
}
