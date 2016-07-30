using System;

namespace WordExtraction
{
    internal class ScanWindow : IScanWindow
    {
        private readonly Predicate<SymbolType?> ALetterOrHebrewLetter = (SymbolType? type) => (type == SymbolType.ST_ALETTER || type == SymbolType.ST_HEBREW_LETTER);

        private readonly Predicate<SymbolType?> EndOfLine = (SymbolType? type) => ( type == SymbolType.ST_NEWLINE 
                                                                                ||  type == SymbolType.ST_LF 
                                                                                ||  type == SymbolType.ST_CR);


        public SymbolType? Ahead { get; private set; }

        public SymbolType? Current { get; private set; }

        public SymbolType? Behind { get; private set; }

        public SymbolType? BehindOfBehind { get; private set; }

        private void MoveWindow()
        {
            BehindOfBehind = Behind;
            Behind = Current;
            Current = Ahead;
        }

        public void AddSymbol(char symbol)
        {
            MoveWindow();
            Ahead = SymbolTable.GetSymbolType(symbol);
        }

        public void AddEmpty()
        {
            MoveWindow();
            Ahead = null;
        }

        public bool CheckForBreak()
        {
            bool result;

            // WB3.
            if (EndOfLine(Behind) || EndOfLine(Current))
                result = !((Behind == SymbolType.ST_CR) && (Current == SymbolType.ST_LF));
            // WB5.
            else if (ALetterOrHebrewLetter(Behind) && ALetterOrHebrewLetter(Current))
                result = false;
            // WB6.
            else if (ALetterOrHebrewLetter(Behind) &&
                    (Current == SymbolType.ST_MIDLETTER || Current == SymbolType.ST_MIDNUMLET || Current == SymbolType.ST_SINGLE_QUOTE) &&
                     ALetterOrHebrewLetter(Ahead))
                result = false;
            // WB7.
            else if (ALetterOrHebrewLetter(BehindOfBehind) &&
                    (Behind == SymbolType.ST_MIDLETTER || Behind == SymbolType.ST_MIDNUMLET || Behind == SymbolType.ST_SINGLE_QUOTE) &&
                    ALetterOrHebrewLetter(Current))
                result = false;
            // WB7a.
            else if (Behind == SymbolType.ST_HEBREW_LETTER && Current == SymbolType.ST_SINGLE_QUOTE)
                return false;
            // WB7b.
            else if (Behind == SymbolType.ST_HEBREW_LETTER &&
                     Current == SymbolType.ST_DOUBLE_QUOTE &&
                     Ahead == SymbolType.ST_HEBREW_LETTER)
                result = false;
            // WB7c.
            else if (BehindOfBehind == SymbolType.ST_HEBREW_LETTER &&
                     Behind == SymbolType.ST_DOUBLE_QUOTE &&
                     Current == SymbolType.ST_HEBREW_LETTER)
                result = false;
            // WB8.
            else if (Behind == SymbolType.ST_NUMERIC &&
                    (Current == SymbolType.ST_NUMERIC))
                result = false;
            // WB9.
            else if (ALetterOrHebrewLetter(Behind) &&
                     Current == SymbolType.ST_NUMERIC)
                result = false;
            // WB10.
            else if (Behind == SymbolType.ST_NUMERIC &&
                     ALetterOrHebrewLetter(Current))
                result = false;
            // WB11.
            else if (BehindOfBehind == SymbolType.ST_NUMERIC &&
                    (Behind == SymbolType.ST_MIDNUM || Behind == SymbolType.ST_MIDNUMLET || Behind == SymbolType.ST_SINGLE_QUOTE) &&
                    Current == SymbolType.ST_NUMERIC)
                result = false;
            // WB12.
            else if (Behind == SymbolType.ST_NUMERIC &&
                    (Current == SymbolType.ST_MIDNUM || Current == SymbolType.ST_MIDNUMLET || Current == SymbolType.ST_SINGLE_QUOTE) &&
                    Ahead == SymbolType.ST_NUMERIC)
                result = false;
            // WB13.
            else if (Behind == SymbolType.ST_KATAKANA &&
                     Current == SymbolType.ST_KATAKANA)
                result = false;
            // WB13a.
            else if ((ALetterOrHebrewLetter(Behind) || Behind == SymbolType.ST_NUMERIC || Behind == SymbolType.ST_KATAKANA || Behind == SymbolType.ST_EXTENDNUMLET) &&
                      Current == SymbolType.ST_EXTENDNUMLET)
                result = false;
            // WB13b.
            else if ((Behind == SymbolType.ST_EXTENDNUMLET) &&
                      (ALetterOrHebrewLetter(Current) || Current == SymbolType.ST_NUMERIC || Current == SymbolType.ST_KATAKANA))
                result = false;
            // WB14.
            else
                result = true;

            return result;
        }

        public ScanWindow(char firstSymbol)
        {
            AddSymbol(firstSymbol);
        }
    }
}
