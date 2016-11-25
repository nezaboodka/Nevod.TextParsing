namespace TextParser
{
    internal class WordBreakerState
    {
        private WordBreak fNextOfNextWordBreak = WordBreak.Empty;
        private WordBreak fNextWordBreak = WordBreak.Empty;
        private WordBreak fCurrentWordBreak = WordBreak.Empty;
        private WordBreak fPreviousWordBreak = WordBreak.Empty;

        private readonly string fText;
        private int fCurrentPosition;

        // Internals

        internal WordBreakerState(string text)
        {
            fText = text;
            fCurrentPosition = -1;
            InitializeLookaheadBuffer();
        }

        internal void InitializeLookaheadBuffer()
        {
            NextCharacter();
            NextCharacter();
        }

        internal void NextCharacter()
        {            
            if (!IsLastCharacter())
            {
                fCurrentPosition++;
                WordBreak wordBreak = WordBreakTable.GetCharacterWordBreak(fText[fCurrentPosition]);
                if (!ShoudIgnore(wordBreak)) // WB4.
                {
                    AddWordBreak(wordBreak);
                }                
            }
            else
            {
                ShiftLookaheadBuffer();
            }
            
        }

        internal bool IsBreak()
        {
            bool result;
            // WB2.
            if (!IsEnoughCharacters() && !IsLastCharacter())
                result = false;            
            // WB3.
            else if (IsLineBreak(fCurrentWordBreak) || IsLineBreak(fNextWordBreak))
                result = !((fCurrentWordBreak == WordBreak.CarriageReturn) && (fNextWordBreak == WordBreak.LineFeed));
            // WB5.
            else if (IsAlphabeticOrHebrewLetter(fCurrentWordBreak) && IsAlphabeticOrHebrewLetter(fNextWordBreak))
                result = false;
            // WB6. (without single quotes)
            else if (IsAlphabeticOrHebrewLetter(fCurrentWordBreak) && (fNextWordBreak == WordBreak.MidLetter ||
                fNextWordBreak == WordBreak.MidNumberAndLetter) 
                && IsAlphabeticOrHebrewLetter(fNextOfNextWordBreak))
                result = false;
            // WB7. (without single quotes)
            else if (IsAlphabeticOrHebrewLetter(fPreviousWordBreak) && (fCurrentWordBreak == WordBreak.MidLetter
                || fCurrentWordBreak == WordBreak.MidNumberAndLetter)
                && IsAlphabeticOrHebrewLetter(fNextWordBreak))
                result = false;
            // WB7a.
            else if (fCurrentWordBreak == WordBreak.HebrewLetter && fNextWordBreak == WordBreak.SingleQuote)
                result = false;
            // WB7b.
            else if (fCurrentWordBreak == WordBreak.HebrewLetter && fNextWordBreak == WordBreak.DoubleQuote
                && fNextOfNextWordBreak == WordBreak.HebrewLetter)
                result = false;
            // WB7c.
            else if (fPreviousWordBreak == WordBreak.HebrewLetter && fCurrentWordBreak == WordBreak.DoubleQuote 
                && fNextWordBreak == WordBreak.HebrewLetter)
                result = false;
            // WB8.
            else if (fCurrentWordBreak == WordBreak.Numeric && (fNextWordBreak == WordBreak.Numeric))
                result = false;
            // WB9.
            else if (IsAlphabeticOrHebrewLetter(fCurrentWordBreak) && fNextWordBreak == WordBreak.Numeric)
                result = false;
            // WB10.
            else if (fCurrentWordBreak == WordBreak.Numeric && IsAlphabeticOrHebrewLetter(fNextWordBreak))
                result = false;
            // WB13.
            else if (fCurrentWordBreak == WordBreak.Katakana && fNextWordBreak == WordBreak.Katakana)
                result = false;
            // WB13a.
            else if ((IsAlphabeticOrHebrewLetter(fCurrentWordBreak) || fCurrentWordBreak == WordBreak.Numeric 
                || fCurrentWordBreak == WordBreak.Katakana || fCurrentWordBreak == WordBreak.ExtenderForNumbersAndLetters) 
                && fNextWordBreak == WordBreak.ExtenderForNumbersAndLetters)
                result = false;
            // WB13b.
            else if ((fCurrentWordBreak == WordBreak.ExtenderForNumbersAndLetters) && (IsAlphabeticOrHebrewLetter(fNextWordBreak)
                || fNextWordBreak == WordBreak.Numeric || fNextWordBreak == WordBreak.Katakana))
                result = false;
            // custom: do not break between whitespaces
            else if ((fCurrentWordBreak == WordBreak.Whitespace) && (fNextWordBreak == WordBreak.Whitespace))
                result = false;
            // WB14.
            else
                result = true;
            return result;
        }

        private void AddWordBreak(WordBreak wordBreak)
        {
            ShiftLookaheadBuffer();
            fNextOfNextWordBreak = wordBreak;
        }

        private void ShiftLookaheadBuffer()
        {
            fPreviousWordBreak = fCurrentWordBreak;
            fCurrentWordBreak = fNextWordBreak;
            fNextWordBreak = fNextOfNextWordBreak;
            fNextOfNextWordBreak = WordBreak.Empty;
        }

        private bool IsEnoughCharacters()
        {
            return (fNextWordBreak != WordBreak.Empty) && (fCurrentWordBreak != WordBreak.Empty);
        }

        private bool IsLastCharacter()
        {
            return fCurrentPosition == fText.Length - 1;
        }

        private bool IsFirstCharacter()
        {
            return (fPreviousWordBreak == WordBreak.Empty) && (fNextOfNextWordBreak == WordBreak.Empty);
        }

        private bool ShoudIgnore(WordBreak wordBreak)
        {
            return ((wordBreak == WordBreak.Extender) || (wordBreak == WordBreak.Format)) && !IsFirstCharacter();
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