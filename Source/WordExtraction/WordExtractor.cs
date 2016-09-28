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
}
