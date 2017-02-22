namespace TextParser
{
   internal enum WordBreak
    {
        AlphabeticLetter,
        Any,
        CarriageReturn,
        DoubleQuote,
        Empty,
        Extender,
        ExtenderForNumbersAndLetters,
        Format,
        HebrewLetter,
        Katakana,
        LineFeed,
        MidLetter,
        MidNumber,
        MidNumberAndLetter,
        Newline,
        Numeric,
        SingleQuote,
        Whitespace,
    }

    internal static class WordBreakTable
    {
        private const WordBreak AL = WordBreak.AlphabeticLetter;
        private const WordBreak W = WordBreak.Whitespace;
        private const WordBreak K = WordBreak.Katakana;
        private const WordBreak F = WordBreak.Format;
        private const WordBreak ML = WordBreak.MidLetter;
        private const WordBreak DQ = WordBreak.DoubleQuote;
        private const WordBreak MN = WordBreak.MidNumber;
        private const WordBreak CR = WordBreak.CarriageReturn;
        private const WordBreak SQ = WordBreak.SingleQuote;
        private const WordBreak ENL = WordBreak.ExtenderForNumbersAndLetters;
        private const WordBreak HL = WordBreak.HebrewLetter;
        private const WordBreak NM = WordBreak.Numeric;
        private const WordBreak E = WordBreak.Extender;
        private const WordBreak NL = WordBreak.Newline;
        private const WordBreak A = WordBreak.Any;
        private const WordBreak LF = WordBreak.LineFeed;
        private const WordBreak MNL = WordBreak.MidNumberAndLetter;
        private static readonly WordBreak[] AlphabeticLetterArray = {};
        private static readonly WordBreak[] AnyArray = {};
        
        private static readonly WordBreak[][] WordBreaks =
        {
            new[]
            {
                A, A, A, A, A, A, A, A, A, W, LF, NL, NL, CR, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, W, A, DQ, A, A, A, A, SQ, A, A, A, A, MN, A, MNL, A, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, ML,
                MN, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, A, A, A, A, ENL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, A, A, A, A, A, A, A, A, A, A, NL, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, AL, A, A, F, A, A, A, A, A,
                A, A, AL, A, ML, A, A, AL, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, AL
            },
            AlphabeticLetterArray,
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, A, A, A, A, A, ML, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, A, A, A, A, A, A, A, AL, A, AL,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E,
                E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E,
                E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E,
                E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, AL, AL, AL, AL, AL, A, AL,
                AL, A, A, AL, AL, AL, AL, MN, A, A, A, A, A, A, A, AL, ML, AL, AL, AL, A, AL, A, AL, AL, AL, AL, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL
            },
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, E, E, E, E, E, E, E, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL
            },
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, A, A,
                AL, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, MN, A, A, A, A, A, A, A, E, E, E,
                E,
                E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E,
                E, E, E, E, E, E, E, E, E, E, E, A, E, A, E, E, A, E, E, A, E, A, A, A, A, A, A, A, A, HL,
                HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL,
                A, A, A, A,
                A, HL, HL, HL, AL, ML, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                F, F, F, F, F, A, A, A, A, A, A, A, MN, MN, A, A, E, E, E, E, E, E, E, E, E, E, E, A, F,
                A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E, E, E, E, E, E, E, E, E, E, E,
                E, E, E, E, E, E, E, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, A, NM, MN, A, AL, AL, E, AL, AL, AL, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, A, AL, E, E, E, E, E, E, E, F, A, E, E, E, E, E, E, AL, AL, E, E, A, E, E, E, E, AL,
                AL, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, AL, AL, AL, A, A, AL
            },
            new[]
            {
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, F, AL, E, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E, E, E, E, E, E, E,
                E,
                E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E, E, E, E, E, E, E, E, AL,
                A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, AL, AL, AL, AL, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                E, E, E, E,
                E, E, E, E, E, AL, AL, A, A, MN, A, AL, A, A, A, A, A
            },
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E, AL,
                E, E,
                E, E, E, E, E, E, E, AL, E, E, E, AL, E, E, E, E, E, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                E, E, E, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, AL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, E, E, E, E, E, E, E, E, E, E, E,
                E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, A
            },
            new[]
            {
                E, E, E, E, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, E,
                E, E, AL, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, AL, E, E, E, E, E, E, E, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, A, AL, AL, AL,
                AL, AL, AL,
                AL, A, AL, AL, AL, AL, AL, AL, AL, A, E, E, E, A, AL, AL, AL, AL, AL, AL, AL, AL, A, A, AL, AL, A, A, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL,
                AL, AL, A, AL,
                A, A, A, AL, AL, AL, AL, A, A, E, AL, E, E, E, E, E, E, E, A, A, E, E, A, A, E, E, E, AL, A, A,
                A, A, A, A, A, A, E, A, A, A, A, AL, AL, A, AL, AL, AL, E, E, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM,
                NM, AL, AL, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                A, E, E, E, A, AL, AL, AL, AL, AL, AL, A, A, A, A, AL, AL, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, A, AL, AL, A,
                AL, AL, A,
                A, E, A, E, E, E, E, E, A, A, A, A, E, E, A, A, E, E, E, A, A, A, E, A, A, A, A, A, A, A,
                AL, AL, AL, AL, A, AL, A, A, A, A, A, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, E, E, AL, AL, AL, E,
                A,
                A, A, A, A, A, A, A, A, A, A, E, E, E, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, A, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL,
                AL, AL, A, AL,
                AL, A, AL, AL, AL, AL, AL, A, A, E, AL, E, E, E, E, E, E, E, E, A, E, E, E, A, E, E, E, A, A, AL,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, AL, AL, E, E, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM,
                NM, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                A, E, E, E, A, AL, AL, AL, AL, AL, AL, AL, AL, A, A, AL, AL, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, A, AL, AL, AL,
                AL, AL, A,
                A, E, AL, E, E, E, E, E, E, E, A, A, E, E, A, A, E, E, E, A, A, A, A, A, A, A, A, E, E, A,
                A, A, A, AL, AL, A, AL, AL, AL, E, E, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, A, AL, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, E, AL, A, AL, AL, AL, AL, AL, AL, A, A, A, AL, AL, AL, A, AL, AL, AL,
                AL, A, A, A, AL, AL, A, AL, A, AL, AL, A, A, A, AL, AL, A, A, A, AL, AL, AL, A, A, A, AL, AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, A, A, A, A, E, E, E, E, E, A, A, A, E, E, E, A, E, E, E, E, A, A, AL,
                A, A, A, A, A, A, E, A, A, A, A, A, A, A, A, A, A, A, A, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM,
                NM, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                A, E, E, E, A, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL,
                AL, AL, A,
                A, A, AL, E, E, E, E, E, E, E, A, E, E, E, A, E, E, E, E, A, A, A, A, A, A, A, E, E, A, AL,
                AL, A, A, A, A, A, A, AL, AL, E, E, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, E, E, A, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, A, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, A, AL, AL, AL, AL, AL, A, A, E, AL, E, E, E, E, E, E, E, A, E, E, E, A, E, E, E, E, A, A, A,
                A, A, A, A, E, E, A, A, A, A, A, A, A, AL, A, AL, AL, E, E, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM,
                NM, A, AL, AL, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                A, A, E, E, A, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                A, A, AL, E, E, E, E, E, E, E, A, E, E, E, A, E, E, E, E, AL, A, A, A, A, A, A, A, A, E, A,
                A, A, A, A, A, A, A, AL, AL, E, E, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, A, A, A, A, A, A, A,
                A, A, A, AL, AL, AL, AL, AL, AL, A, A, E, E, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, A,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, A, A, AL, AL, AL, AL, AL, AL, AL, A, A, A, E, A, A, A, A, E,
                E,
                E, E, E, E, A, E, A, E, E, E, E, E, E, E, E, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, E, E, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, E, A, A, E, E, E, E, E, E, E,
                A, A, A, A, A, A, A, A, A, A, A, A, E, E, E, E, E, E, E, E, A, NM, NM, NM, NM, NM, NM, NM, NM, NM,
                NM, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, E, A,
                A, E, E, E, E, E, E, A, E, E, A, A, A, A, A, A, A, A, A, A, A, E, E, E, E, E, E, A, A, NM,
                NM, NM, NM, NM, NM, NM, NM, NM, NM, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                AL, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, E, E, A, A, A,
                A, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, A, A, A, A, A, A, A, A, A, A, A, E, A, E, A, E, A,
                A, A, A, E, E, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, E, E, E, E,
                E, E,
                E, E, E, E, E, E, E, E, E, E, E, E, E, E, A, E, E, AL, AL, AL, AL, AL, E, E, E, E, E, E, E, E,
                E, E, E, A, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E,
                E, E, E, E, E, E, E, E, E, E, A, A, A, A, A, A, A, A, A, E, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E,
                E, E, E, E, A, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, A, A, A, A, A, A, A, A, A, A, A, A, E, E, E,
                E, A, A, A, A, E, E, E, A, E, E, E, A, A, E, E, E, E, E, E, E, A, A, A, E, E, E, E, A, A,
                A, A, A, A, A, A, A, A, A, A, A, E, E, E, E, E, E, E, E, E, E, E, E, A, E, NM, NM, NM, NM, NM,
                NM, NM, NM, NM, NM, E, E, E, E, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, A, A, A, A, A, AL, A,
                A, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL
            },
            AlphabeticLetterArray,
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, A, A, AL, AL, AL, AL, AL, AL,
                AL, A, AL,
                A, AL, AL, AL, AL, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, A, A, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, A, AL,
                AL, AL, AL, A, A, AL, AL, AL, AL, AL, AL, AL, A, AL, A, AL, AL, AL, AL, A, A, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL
            },
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, A, A, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, A, A, E, E, E, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL
            },
            AlphabeticLetterArray,
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                A, A, A, AL,
                AL, AL, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, E, E, E, A, A, A, A, A, A, A, A,
                A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, A, A, A, A, A,
                A,
                A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, A, A, A, A,
                A,
                A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, A, E, E, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E,
                E, E, E, A, A, A, A, A, A, A, A, A, E, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                A, A, A, A, A, A, A, A, A, A, A, E, E, E, F, A, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, A, A, A,
                A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, AL, A, A, A, A, A, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                A, A, A, E, E, E, E, E, E, E, E, E, E, E, E, A, A, A, A, E, E, E, E, E, E, E, E, E, E, E,
                E, A, A, A, A, A, A, A, A, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, E, E, E,
                E, E, E, E, E, E, E, E, E, E, E, E, E, E, A, A, A, A, A, A, A, E, E, A, A, A, A, A, A, NM,
                NM, NM, NM, NM, NM, NM, NM, NM, NM, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E,
                E, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, E, E, E, E,
                E, E, E, E, E, E, A, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E,
                E, E, E, E, E, E, A, A, E, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, A, A, A, A, A, A, NM, NM, NM, NM, NM,
                NM, NM, NM, NM, NM, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                E, E, E, E, E, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E,
                E, E, E,
                E, E, E, E, E, E, E, E, E, E, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM,
                NM, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, E, E, E, E, E, E, E, E, E, A, A, A,
                A, A, A, A, A, A, A, A, A, E, E, E, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E, E, E, E, E, E, E, E, E, E, AL, AL, NM, NM,
                NM,
                NM, NM, NM, NM, NM, NM, NM, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E, E, E, E,
                E, E,
                E, E, E, E, E, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, A, A, A,
                A, A, A, A, A, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, A, A, A, AL, AL, AL, NM, NM, NM, NM, NM, NM, NM,
                NM, NM,
                NM, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, E,
                E, E, A, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, AL, AL, AL, AL, E, AL,
                AL, AL, AL, E, E, E, AL, AL, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E,
                E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, E, E, E, E
            },
            AlphabeticLetterArray,
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, AL, AL, AL,
                AL, AL,
                AL, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, AL, AL, AL, AL, AL, AL, A, A, AL, AL, AL, AL, AL, AL,
                AL, AL, A,
                AL, A, AL, A, AL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, A, AL, AL, AL, AL, AL, AL, AL, A, AL, A, A, A, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, A, A,
                A, AL,
                AL, AL, AL, A, A, AL, AL, AL, AL, AL, AL, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                A, A,
                A, A, A, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, A, A, A
            },
            new[]
            {
                A, A, A, A, A, A, A, A, A, A, A, A, E, E, F, F, A, A, A, A, A, A, A, A, MNL, MNL, A, A, A,
                A, A, A, A, A, A, A, MNL, A, A, ML, NL, NL, F, F, F, F, F, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, ENL, ENL, A, A, A, MN, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, ENL, A, A, A, A,
                A, A, A, A, A, A, A, F, F, F, F, F, A, F, F, F, F, F, F, F, F, F, F, A, AL, A, A, A, A, A,
                A, A, A, A, A, A, A, A, AL, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, E,
                E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E,
                E, E, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                A, A, AL, A, A, A, A, AL, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, A, A, A, AL, AL, AL, AL,
                AL, A, A, A, A, A, A, AL, A, AL, A, AL, A, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                A,
                A, AL, AL, AL, AL, A, A, A, A, A, AL, AL, AL, AL, AL, A, A, A, A, AL, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A, A, A, A, A, A,
                A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            AnyArray, AnyArray,
            new[]
            {
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A,
                A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A, AL, AL,
                AL, AL,
                E, E, E, AL, AL, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, A, AL, A, A, A, A, A, AL, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A, A, AL, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, E, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, A, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, A, AL, AL,
                AL,
                AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL,
                AL, A, AL,
                AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, A, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E,
                E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E
            },
            new[]
            {
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, AL, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            AnyArray,
            new[]
            {
                A, A, A, A, A, AL, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, E, E, E, E, E, E, A, K, K, K, K, K, A, A, A, A, A,
                AL, AL, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, E, E, K, K, A, A, A, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K,
                K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K,
                K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K,
                K, K, K, K, K, K, K, K, K, K, K, K, A, K, K, K, K
            },
            new[]
            {
                A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A,
                A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K
            },
            new[]
            {
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, K,
                K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K,
                K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, A
            },
            new[]
            {
                K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K,
                K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K,
                K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AlphabeticLetterArray,
            AlphabeticLetterArray,
            AlphabeticLetterArray, AlphabeticLetterArray,
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A,
                A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A
            },
            AlphabeticLetterArray,
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, AL, AL, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E, A, E,
                E, E,
                E, E, E, E, E, E, E, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, A, A, A, A, A, A, A, E, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, E, E, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, AL, AL, AL, AL, A, AL, AL,
                AL, AL, A,
                A, A, A, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL
            },
            new[]
            {
                AL, AL, E, AL, AL, AL, E, AL, AL, AL, AL, E, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, E, E, E, E, E, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, A, A, A,
                A, A, A, A, A, A, A, A, A, E, E, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, A, A, A, A, A, A, A, A, A, A, A, NM,
                NM, NM, NM, NM, NM, NM, NM, NM, NM, A, A, A, A, A, A, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E,
                E, E, E, AL, AL, AL, AL, AL, AL, A, A, A, AL, A, A, A, A
            },
            new[]
            {
                NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E, E, E, E, E, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E, E, E, E, E, E, E, E, E, E, A, A, A, A, A,
                A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, A, A, A, E, E, E, E, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                E, E, E, E, E, E, E, E, E, E, E, E, E, E, A, A, A, A, A, A, A, A, A, A, A, A, A, A, AL, NM,
                NM, NM, NM, NM, NM, NM, NM, NM, NM, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E, E, E, E, E, E, E, E, E, E, E, A, A, A, A,
                A, A, A, A, A, AL, AL, AL, E, AL, AL, AL, AL, AL, AL, AL, AL, E, E, A, A, NM, NM, NM, NM, NM, NM, NM, NM,
                NM,
                NM, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, E, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, E, A, E,
                E, E, A, A, E, E, A, A, A, A, A, E, E, A, E, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E,
                E, A, A, AL, AL, AL, E, E, A, A, A, A, A, A, A, A, A
            },
            new[]
            {
                A, AL, AL, AL, AL, AL, AL, A, A, AL, AL, AL, AL, AL, AL, A, A, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A,
                A, A, A, AL, AL, AL, AL, AL, AL, AL, A, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, E, E, E, E, E, E, E, E, A, E, E,
                A,
                A, NM, NM, NM, NM, NM, NM, NM, NM, NM, NM, A, A, A, A, A, A
            },
            AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray,
            AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray,
            AlphabeticLetterArray, AlphabeticLetterArray,
            AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray,
            AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray,
            AlphabeticLetterArray, AlphabeticLetterArray,
            AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray,
            AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray,
            AlphabeticLetterArray, AlphabeticLetterArray,
            AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray,
            AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray,
            AlphabeticLetterArray, AlphabeticLetterArray,
            AlphabeticLetterArray, AlphabeticLetterArray, AlphabeticLetterArray,
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A, A, A, A, A, A, A, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A
            },
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            AnyArray, AnyArray, AnyArray, AnyArray, AnyArray,
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, A, A, A, A, A,
                HL, E, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, A, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, A,
                HL, HL, HL,
                HL, HL, A, HL, A, HL, HL, A, HL, HL, A, HL, HL, HL, HL, HL, HL, HL, HL, HL, HL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL
            },
            AlphabeticLetterArray,
            new[]
            {
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A,
                AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A, A, A, A,
                A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, A
            },
            new[]
            {
                E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, MN, A, A, ML, MN, A, A, A, A, A, A, A, A,
                A, A, A, E, E, E, E, E, E, E, A, A, A, A, A, A, A, A, A, A, A, A, ENL, ENL, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, ENL, ENL, ENL, MN, A, MNL, A, MN, ML, A, A, A,
                A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, AL, AL, AL, AL, AL, A, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL, AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, F
            },
            new[]
            {
                A, A, A, A, A, A, A, MNL, A, A, A, A, MN, A, MNL, A, A, A, A, A, A, A, A, A, A, A, ML, MN, A,
                A, A, A, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                A, A, A, A, ENL, A, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL, AL, AL,
                AL, AL, A, A, A, A, A, A, A, A, A, A, A, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K,
                K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K, K,
                K, K, K, K, K, K, K, K, K, E, E, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL,
                AL,
                AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, AL, A, A, A, AL, AL, AL, AL, AL, AL, A, A, AL, AL, AL, AL,
                AL, AL, A,
                A, AL, AL, AL, AL, AL, AL, A, A, AL, AL, AL, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A,
                A, A, A, A, A, A, A, A, A, A, F, F, F, A, A, A, A
            }
        };

        // Public

        public static WordBreak GetCharacterWordBreak(char c)
        {
            WordBreak result;
            int high = c >> 8;
            WordBreak[] lowArray = WordBreaks[high];
            if (lowArray == AlphabeticLetterArray)
            {
                result = WordBreak.AlphabeticLetter;
            } else if (lowArray == AnyArray)
            {
                result = WordBreak.Any;
            }
            else
            {
                result = lowArray[c & 0x00FF];
            }            
            return result;
        }
    }
}
