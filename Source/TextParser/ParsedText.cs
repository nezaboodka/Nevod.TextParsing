using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Sharik.Text;

namespace TextParser
{
    public class ParsedText
    {

        private List<Token> fTokens;
        private List<Tag> fTags;        
        private List<string> fXhtmlElements;
        internal List<int> fPlainTextInXhtml;

        // Public

        public List<Token> Tokens => fTokens;
        public IEnumerable<Tag> Tags  => fTags;
        public List<string> XhtmlElements => fXhtmlElements;

        public ParsedText()
        {
            fTokens = new List<Token>();
            fTags = new List<Tag>();
            fXhtmlElements = new List<string>();
            fPlainTextInXhtml = new List<int>();
        }

        public string GetAllPlainText()
        {
            StringBuilder plainTextBuilder = new StringBuilder();
            foreach (int plainTextElementIndex in fPlainTextInXhtml)
            {
                plainTextBuilder.Append(fXhtmlElements[plainTextElementIndex]);
            }
            return plainTextBuilder.ToString();
        }

        public string GetPlainText(Token token)
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

        // Internal

        internal void AddToken(Token token)
        {
            if ((token.XhtmlIndex < fXhtmlElements.Count) || (token.XhtmlIndex < 0))
            {
                fTokens.Add(token);
            }
            else
            {
                throw new ArgumentException("Can't add token with invalid XhtmlIndex.");
            }
        }

        internal void AddTag(Tag tag)
        {
            fTags.Add(tag);
        }

        internal void AddXhtmlElement(string xhtmlElement, bool isPlainText)
        {
            fXhtmlElements.Add(xhtmlElement);
            if (isPlainText)
            {
                fPlainTextInXhtml.Add(fXhtmlElements.Count - 1);               
            }            
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