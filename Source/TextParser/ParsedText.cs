using System;
using System.Collections;
using System.Collections.Generic;
using Sharik.Text;

namespace TextParser
{
    public class ParsedText : IEnumerable<Token>
    {
        private readonly List<int> fTokenEnds;
        private readonly List<TokenKind> fTokenKinds;

        // Public
        
        public virtual string PlainText { get; }
        public virtual int Count => fTokenKinds.Count;
        public virtual Token this[int index]
        {
            get
            {
                Slice text = PlainText.Slice(fTokenEnds[index] + 1, fTokenEnds[index + 1] - fTokenEnds[index]);
                return new Token(text, fTokenKinds[index]);
            }
        }

        public IEnumerator<Token> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Internal

        internal ParsedText(string plainText)
        {
            PlainText = plainText;
            fTokenEnds = new List<int> { -1 };
            fTokenKinds = new List<TokenKind>();
        }

        internal virtual void AddToken(int tokenEnd, TokenKind kind)
        {
            if (tokenEnd < 0 || tokenEnd >= PlainText.Length)
            {
                throw new IndexOutOfRangeException("Invalid token token end position");
            }
            fTokenEnds.Add(tokenEnd);
            fTokenKinds.Add(kind);
        }
    }
}