using System.Collections.Generic;
using System.Linq;
using Sharik.Text;

namespace WordExtraction
{
    public class WordExtractor
    {
        private WordBreak fAhead = WordBreak.Empty;
        private WordBreak fCurrent = WordBreak.Empty;
        private WordBreak fBehind = WordBreak.Empty;
        private WordBreak fBehindOfBehind = WordBreak.Empty;

        public virtual IEnumerable<Slice> GetWords(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                AddSymbol(text[0]);
                int startPosition = 0;
                bool isWord = false;
                for (int i = 0; i < text.Length - 1; i++)
                {
                    AddSymbol(text[i + 1]);
                    if (CheckForBreak())
                    {
                        if (isWord)
                            yield return SliceUtilities.Slice(text, startPosition, i - startPosition);
                        startPosition = i;
                        isWord = false;
                    }
                    if (!isWord && char.IsLetterOrDigit(text[i]))
                        isWord = true;
                }
                MoveWindow();
                if (CheckForBreak())
                {
                    if (isWord)
                        yield return SliceUtilities.Slice(text, startPosition, text.Length - startPosition - 1);
                    startPosition = text.Length - 1;
                }
                if (char.IsLetterOrDigit(text.Last()))
                {
                    yield return SliceUtilities.Slice(text, startPosition);
                }
            }
        }

        public virtual void AddSymbol(char symbol)
        {
            MoveWindow();
            fAhead = WordBreakTable.GetSymbolType(symbol);
        }

        public virtual bool CheckForBreak()
        {
            bool result;
            // WB3.
            if (IsLineBreak(fBehind) || IsLineBreak(fCurrent))
                result = !((fBehind == WordBreak.CarriageReturn) && (fCurrent == WordBreak.LineFeed));
            // WB5.
            else if (IsAlphabeticOrHebrewLetter(fBehind) && IsAlphabeticOrHebrewLetter(fCurrent))
                result = false;
            // WB6.
            else if (IsAlphabeticOrHebrewLetter(fBehind) && (fCurrent == WordBreak.MidLetter ||
                fCurrent == WordBreak.MidNumberAndLetter || fCurrent == WordBreak.SingleQuote) && IsAlphabeticOrHebrewLetter(fAhead))
                result = false;
            // WB7.
            else if (IsAlphabeticOrHebrewLetter(fBehindOfBehind) && (fBehind == WordBreak.MidLetter
                || fBehind == WordBreak.MidNumberAndLetter || fBehind == WordBreak.SingleQuote)
                && IsAlphabeticOrHebrewLetter(fCurrent))
                result = false;
            // WB7a.
            else if (fBehind == WordBreak.HebrewLetter && fCurrent == WordBreak.SingleQuote)
                return false;
            // WB7b.
            else if (fBehind == WordBreak.HebrewLetter && fCurrent == WordBreak.DoubleQuote
                && fAhead == WordBreak.HebrewLetter)
                result = false;
            // WB7c.
            else if (fBehindOfBehind == WordBreak.HebrewLetter && fBehind == WordBreak.DoubleQuote &&
                fCurrent == WordBreak.HebrewLetter)
                result = false;
            // WB8.
            else if (fBehind == WordBreak.Numeric && (fCurrent == WordBreak.Numeric))
                result = false;
            // WB9.
            else if (IsAlphabeticOrHebrewLetter(fBehind) && fCurrent == WordBreak.Numeric)
                result = false;
            // WB10.
            else if (fBehind == WordBreak.Numeric && IsAlphabeticOrHebrewLetter(fCurrent))
                result = false;
            // WB11.
            else if (fBehindOfBehind == WordBreak.Numeric &&
                (fBehind == WordBreak.MidNumber || fBehind == WordBreak.MidNumberAndLetter || fBehind == WordBreak.SingleQuote) &&
                fCurrent == WordBreak.Numeric)
                result = false;
            // WB12.
            else if (fBehind == WordBreak.Numeric && (fCurrent == WordBreak.MidNumber || fCurrent == WordBreak.MidNumberAndLetter
                || fCurrent == WordBreak.SingleQuote) && fAhead == WordBreak.Numeric)
                result = false;
            // WB13.
            else if (fBehind == WordBreak.Katakana && fCurrent == WordBreak.Katakana)
                result = false;
            // WB13a.
            else if ((IsAlphabeticOrHebrewLetter(fBehind) || fBehind == WordBreak.Numeric || fBehind == WordBreak.Katakana
                || fBehind == WordBreak.ExtenderForNumbersAndLetters) && fCurrent == WordBreak.ExtenderForNumbersAndLetters)
                result = false;
            // WB13b.
            else if ((fBehind == WordBreak.ExtenderForNumbersAndLetters) && (IsAlphabeticOrHebrewLetter(fCurrent)
                || fCurrent == WordBreak.Numeric || fCurrent == WordBreak.Katakana))
                result = false;
            // WB14.
            else
                result = true;
            return result;
        }

        public virtual void MoveWindow()
        {
            fBehindOfBehind = fBehind;
            fBehind = fCurrent;
            fCurrent = fAhead;
            fAhead = WordBreak.Empty;
        }        

        private static bool IsAlphabeticOrHebrewLetter(WordBreak symbolType)
        {
            return symbolType == WordBreak.AlphabeticLetter || symbolType == WordBreak.HebrewLetter;
        }

        private static bool IsLineBreak(WordBreak symbolType)
        {
            return symbolType == WordBreak.Newline || symbolType == WordBreak.LineFeed || symbolType == WordBreak.CarriageReturn;
        }              
    }
}
