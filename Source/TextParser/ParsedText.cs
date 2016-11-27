using System;
using System.Collections;
using System.Collections.Generic;
using Sharik.Text;

namespace TextParser
{
    public class ParsedText
    {
        private readonly List<int> fTokenEnds;
        private readonly List<TokenKind> fTokenKinds;

        // Public
        
        public virtual string PlainText { get; }
        public virtual int TokenCount => fTokenKinds.Count;

        public virtual Token GetToken(int index)
        {
            Slice text = PlainText.Slice(fTokenEnds[index] + 1, fTokenEnds[index + 1] - fTokenEnds[index]);
            return new Token(text, fTokenKinds[index]);
        }

        public virtual IEnumerable<Token> Tokens
        {
            get
            {
                for (int i = 0; i < TokenCount; i++)
                {
                    yield return GetToken(i);
                }
            }            
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