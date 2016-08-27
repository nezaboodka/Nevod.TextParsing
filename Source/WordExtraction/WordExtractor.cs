using System.Collections.Generic;
using System.Linq;
using Sharik.Text;

namespace WordExtraction
{
    public class WordExtractor
    {
        public virtual IEnumerable<Slice> GetWords(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                bool isWord = false;
                int startPosition = 0;
                var wordExtractorState = new WordExtractorState();
                wordExtractorState.AddCharacter(source[0]);
                for (int i = 0; i < source.Length - 1; i++)
                {
                    wordExtractorState.AddCharacter(source[i + 1]);
                    if (wordExtractorState.IsBreak())
                    {
                        if (isWord)
                            yield return source.Slice(startPosition, i - startPosition);
                        startPosition = i;
                        isWord = false;
                    }
                    if (!isWord && char.IsLetterOrDigit(source[i]))
                        isWord = true;
                }
                wordExtractorState.NextCharacter();
                if (wordExtractorState.IsBreak())
                {
                    if (isWord)
                        yield return source.Slice(startPosition, source.Length - startPosition - 1);
                    startPosition = source.Length - 1;
                }
                if (char.IsLetterOrDigit(source.Last()))
                {
                    yield return source.Slice(startPosition);
                }
            }
        }         
    }

    internal class WordExtractorState
    {
        private WordBreak fNextWordBreak = WordBreak.Empty;
        private WordBreak fCurrentWordBreak = WordBreak.Empty;
        private WordBreak fPreviousWordBreak = WordBreak.Empty;
        private WordBreak fPreviousOfPreviousWordBreak = WordBreak.Empty;

        // Internals

        internal void AddCharacter(char c)
        {
            WordBreak wordBreak = WordBreakTable.GetCharacterWordBreak(c);
            if (!ShoudIgnore(wordBreak)) // WB4.
            {
                NextCharacter();
                fNextWordBreak = WordBreakTable.GetCharacterWordBreak(c);
            }
        }

        internal bool IsBreak()
        {
            bool result;
            if (!IsEnoughCharacters())
                result = false;
            // WB3.
            else if (IsLineBreak(fPreviousWordBreak) || IsLineBreak(fCurrentWordBreak))
                result = !((fPreviousWordBreak == WordBreak.CarriageReturn) && (fCurrentWordBreak == WordBreak.LineFeed));
            // WB5.
            else if (IsAlphabeticOrHebrewLetter(fPreviousWordBreak) && IsAlphabeticOrHebrewLetter(fCurrentWordBreak))
                result = false;
            // WB6.
            else if (IsAlphabeticOrHebrewLetter(fPreviousWordBreak) && (fCurrentWordBreak == WordBreak.MidLetter ||
                fCurrentWordBreak == WordBreak.MidNumberAndLetter || fCurrentWordBreak == WordBreak.SingleQuote) 
                && IsAlphabeticOrHebrewLetter(fNextWordBreak))
                result = false;
            // WB7.
            else if (IsAlphabeticOrHebrewLetter(fPreviousOfPreviousWordBreak) && (fPreviousWordBreak == WordBreak.MidLetter
                || fPreviousWordBreak == WordBreak.MidNumberAndLetter || fPreviousWordBreak == WordBreak.SingleQuote)
                && IsAlphabeticOrHebrewLetter(fCurrentWordBreak))
                result = false;
            // WB7a.
            else if (fPreviousWordBreak == WordBreak.HebrewLetter && fCurrentWordBreak == WordBreak.SingleQuote)
                return false;
            // WB7b.
            else if (fPreviousWordBreak == WordBreak.HebrewLetter && fCurrentWordBreak == WordBreak.DoubleQuote
                && fNextWordBreak == WordBreak.HebrewLetter)
                result = false;
            // WB7c.
            else if (fPreviousOfPreviousWordBreak == WordBreak.HebrewLetter && fPreviousWordBreak == WordBreak.DoubleQuote 
                && fCurrentWordBreak == WordBreak.HebrewLetter)
                result = false;
            // WB8.
            else if (fPreviousWordBreak == WordBreak.Numeric && (fCurrentWordBreak == WordBreak.Numeric))
                result = false;
            // WB9.
            else if (IsAlphabeticOrHebrewLetter(fPreviousWordBreak) && fCurrentWordBreak == WordBreak.Numeric)
                result = false;
            // WB10.
            else if (fPreviousWordBreak == WordBreak.Numeric && IsAlphabeticOrHebrewLetter(fCurrentWordBreak))
                result = false;
            // WB11.
            else if (fPreviousOfPreviousWordBreak == WordBreak.Numeric &&
                (fPreviousWordBreak == WordBreak.MidNumber || fPreviousWordBreak == WordBreak.MidNumberAndLetter 
                || fPreviousWordBreak == WordBreak.SingleQuote) && fCurrentWordBreak == WordBreak.Numeric)
                result = false;
            // WB12.
            else if (fPreviousWordBreak == WordBreak.Numeric && (fCurrentWordBreak == WordBreak.MidNumber 
                || fCurrentWordBreak == WordBreak.MidNumberAndLetter || fCurrentWordBreak == WordBreak.SingleQuote) 
                && fNextWordBreak == WordBreak.Numeric)
                result = false;
            // WB13.
            else if (fPreviousWordBreak == WordBreak.Katakana && fCurrentWordBreak == WordBreak.Katakana)
                result = false;
            // WB13a.
            else if ((IsAlphabeticOrHebrewLetter(fPreviousWordBreak) || fPreviousWordBreak == WordBreak.Numeric 
                || fPreviousWordBreak == WordBreak.Katakana || fPreviousWordBreak == WordBreak.ExtenderForNumbersAndLetters) 
                && fCurrentWordBreak == WordBreak.ExtenderForNumbersAndLetters)
                result = false;
            // WB13b.
            else if ((fPreviousWordBreak == WordBreak.ExtenderForNumbersAndLetters) && (IsAlphabeticOrHebrewLetter(fCurrentWordBreak)
                || fCurrentWordBreak == WordBreak.Numeric || fCurrentWordBreak == WordBreak.Katakana))
                result = false;
            // WB14.
            else
                result = true;
            return result;
        }

        private bool IsEnoughCharacters()
        {
            return (fCurrentWordBreak != WordBreak.Empty) && (fPreviousWordBreak != WordBreak.Empty);
        }

        internal void NextCharacter()
        {
            fPreviousOfPreviousWordBreak = fPreviousWordBreak;
            fPreviousWordBreak = fCurrentWordBreak;
            fCurrentWordBreak = fNextWordBreak;
            fNextWordBreak = WordBreak.Empty;
        }

        private bool IsFirstCharacter()
        {
            return (fPreviousOfPreviousWordBreak == WordBreak.Empty) && (fNextWordBreak == WordBreak.Empty);
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
