namespace WordExtraction
{
    enum WordBreakPropertyType
    {
        WBPT_ANY,
        WBPT_1,
        WBPT_2,
        WBPT_3,
        /* %types% */
    }

    static class WordBreakPropertyTable
    {
        private static readonly WordBreakProperty[] WORD_BREAKPROPERTIES =  {   new WordBreakProperty('a', 'c', WordBreakPropertyType.WBPT_1),
                                                                                new WordBreakProperty('d', 'e', WordBreakPropertyType.WBPT_2),
                                                                                new WordBreakProperty('h', 'z', WordBreakPropertyType.WBPT_3),
                                                                                /* %properties% */
                                                                            };
        private struct WordBreakProperty
        {
            public char lowBound;
            public char highBound;
            public WordBreakPropertyType wordBreakPropertyType;

            public WordBreakProperty(char lowBound, char highBound, WordBreakPropertyType wordBreakPropertyType)
            {
                this.lowBound = lowBound;
                this.highBound = highBound;
                this.wordBreakPropertyType = wordBreakPropertyType;
            }
        }

        private static int InRange(char c, WordBreakProperty wordBreakProperty)
        {
            int result;

            if ((wordBreakProperty.lowBound <= c) && (c <= wordBreakProperty.highBound))
                result = 0;
            else if (wordBreakProperty.highBound < c)
                result = 1;
            else
                result = -1;

            return result;
        }

        public static WordBreakPropertyType GetWordBreakPropertyType(char c)
        {
            WordBreakPropertyType result = WordBreakPropertyType.WBPT_ANY;

            int beginIndex = 0, endIndex = WORD_BREAKPROPERTIES.Length;
            bool isFound = false;

            while ((beginIndex <= endIndex) && !isFound)
            {
                int i = (beginIndex + endIndex) / 2;
                int comparisonResult = InRange(c, WORD_BREAKPROPERTIES[i]);

                if (comparisonResult == 0)
                {
                    result = WORD_BREAKPROPERTIES[i].wordBreakPropertyType;
                    isFound = true;
                }
                else if (comparisonResult < 0)
                    endIndex = i - 1;
                else
                    beginIndex = i + 1;
            }

            return result;
        }

    }
}