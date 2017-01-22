﻿using System.Collections.Generic;
using System.Text;

namespace TextParser.Common
{
    public class ParsedText
    {

        private readonly List<Token> fTokens;
        private readonly List<Tag> fTags;
        private readonly List<DocumentTag> fDocumentTags;
        private readonly List<string> fXhtmlElements;
        private readonly List<int> fPlainTextInXhtml;

        // Public

        public List<Token> Tokens => fTokens;
        public IEnumerable<Tag> Tags => fTags;
        public IEnumerable<DocumentTag> DocumentTags => fDocumentTags;
        public List<string> XhtmlElements => fXhtmlElements;

        public ParsedText()
        {
            fTokens = new List<Token>();
            fTags = new List<Tag>();
            fXhtmlElements = new List<string>();
            fPlainTextInXhtml = new List<int>();
            fDocumentTags = new List<DocumentTag>();
        }

        public string GetPlainText()
        {
            StringBuilder plainTextBuilder = new StringBuilder();
            foreach (int plainTextElementIndex in fPlainTextInXhtml)
            {
                plainTextBuilder.Append(fXhtmlElements[plainTextElementIndex]);
            }
            return plainTextBuilder.ToString();
        }

        public string GetTokenText(Token token)
        {
            string result;

            if (token.StringPosition + token.StringLength <= fXhtmlElements[token.XhtmlIndex].Length)
            {
                result = fXhtmlElements[token.XhtmlIndex].Substring(token.StringPosition, token.StringLength);
            }
            else
            {
                result = GetCompoundTokenText(token);
            }

            return result;
        }

        public string GetTagText(Tag tag)
        {
            StringBuilder result = new StringBuilder();
            for (int tokenIndex = tag.TokenPosition, i = 0; i < tag.TokenLength; i++, tokenIndex++)
            {
                result.Append(GetTokenText(Tokens[tokenIndex]));
            }
            return result.ToString();
        }

        // Internals

        internal void AddToken(Token token)
        {
            fTokens.Add(token);
        }

        internal void AddTag(Tag tag)
        {
            fTags.Add(tag);
        }

        internal void AddTags(IEnumerable<Tag> tags)
        {
            foreach (Tag tag in tags)
            {
                AddTag(tag);
            }
        }

        internal void AddDocumentTag(DocumentTag documentTag)
        {
            fDocumentTags.Add(documentTag);
        }

        internal void AddXhtmlElement(string xhtmlElement)
        {
            fXhtmlElements.Add(xhtmlElement);            
        }

        internal void AddPlainTextElement(string plainTextElement)
        {
            AddXhtmlElement(plainTextElement);
            fPlainTextInXhtml.Add(fXhtmlElements.Count - 1);
        }

        private string GetCompoundTokenText(Token token)
        {
            StringBuilder tokenTextBuilder = new StringBuilder();
            int currentSubstringLength = fXhtmlElements[token.XhtmlIndex].Length - token.StringPosition;
            tokenTextBuilder.Append(fXhtmlElements[token.XhtmlIndex].Substring(token.StringPosition, currentSubstringLength));
            int copiedLength = currentSubstringLength;            
            int plainTextElement = fPlainTextInXhtml.BinarySearch(token.XhtmlIndex) + 1;

            while (copiedLength < token.StringLength)
            {
                int xhtmlElement = fPlainTextInXhtml[plainTextElement];
                int remainedLength = token.StringLength - copiedLength;                
                if (fXhtmlElements[xhtmlElement].Length <= remainedLength)
                {
                    currentSubstringLength = fXhtmlElements[xhtmlElement].Length;
                }
                else
                {
                    currentSubstringLength = remainedLength;
                }
                tokenTextBuilder.Append(fXhtmlElements[xhtmlElement].Substring(0, currentSubstringLength));
                copiedLength += currentSubstringLength;
                plainTextElement++;
            }

            return tokenTextBuilder.ToString();
        }      
    }
}