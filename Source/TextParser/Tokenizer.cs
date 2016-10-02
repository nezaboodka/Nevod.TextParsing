using System.Collections.Generic;
using System.Linq;
using Sharik.Text;

namespace TextParser
{
    public enum TokenKind
    {
        Word,
        Number,
        Url,
        SentenceSeparator
    }

    public struct Token
    {
        public Slice Text;
        public TokenKind TokenKind;
        public int Position => Text.Position;

        public Token(Slice text)
        {
            Text = text;
            TokenKind = TokenKind.Word;;
        }

        public Token(Slice text, TokenKind tokenKind)
        {
            Text = text;
            TokenKind = tokenKind;
        }
    }

    public class Tokenizer
    {
        public virtual IEnumerable<Token> GetTokens(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                bool isWord = false;
                int startPosition = 0;
                var tokenizerState = new TokenizerState();
                tokenizerState.AddCharacter(source[0]);
                for (int i = 0; i < source.Length - 1; i++)
                {
                    tokenizerState.AddCharacter(source[i + 1]);
                    if (tokenizerState.IsBreak())
                    {
                        if (isWord)
                            yield return new Token(source.Slice(startPosition, i - startPosition));
                        startPosition = i;
                        isWord = false;
                    }
                    if (!isWord && char.IsLetterOrDigit(source[i]))
                        isWord = true;
                }
                tokenizerState.NextCharacter();
                if (tokenizerState.IsBreak())
                {
                    if (isWord)
                        yield return new Token(source.Slice(startPosition, source.Length - startPosition - 1));
                    startPosition = source.Length - 1;
                }
                if (char.IsLetterOrDigit(source.Last()))
                {
                    yield return new Token(source.Slice(startPosition));
                }
            }
        }         
    }
}
