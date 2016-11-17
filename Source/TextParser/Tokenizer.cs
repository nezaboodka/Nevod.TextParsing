using System;
using System.Collections.Generic;
using System.Linq;
using Sharik.Text;

namespace TextParser
{
    public class Tokenizer
    {
        private readonly WordBreakerState fWordBreakerState;
        private readonly TokenizerState fTokenizerState;
        private readonly string fText;
        private int position;

        public static TokenizerResult GetTokensFromPlainText(string text)
        {
            var tokenizer = new Tokenizer(text);
            return tokenizer.GetTokensFromPlainText();
        }

        // Internals

        private Tokenizer(string text)
        {
            fText = text;
            fWordBreakerState = new WordBreakerState();
            fTokenizerState = new TokenizerState();
            position = 0;
        }

        protected virtual TokenizerResult GetTokensFromPlainText()
        {
            var result = new TokenizerResult(fText);

            if (!string.IsNullOrEmpty(fText))
            {
                int startPosition = 0;

                NextCharacter();
                for (position = 0; position < fText.Length - 1; position++)
                {
                    NextCharacter();
                    if (fWordBreakerState.IsBreak())
                    {
                        //yield return new TokenizerResult(source.Slice(startPosition, i - startPosition));
                        startPosition = position;
                    }
                }
                NextCharacter();
                if (fWordBreakerState.IsBreak())
                {
                    //yield return new TokenizerResult(source.Slice(startPosition, source.Length - startPosition - 1));
                    startPosition = fText.Length - 1;
                }
                //yield return new TokenizerResult(source.Slice(startPosition));
            }

            return result;
        }

        protected virtual void NextCharacter()
        {
            if (position < fText.Length - 1)
            {
                fWordBreakerState.AddCharacter(fText[position]);
            }
            else
            {
                fWordBreakerState.NextCharacter();
            }
        }        
    }
}
