using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TextParser.Common;

namespace TextParser.Tests
{
    [TestClass]
    public class ParsedTextTests
    {
        private static readonly string[] Xhtml = { "<p>", "Hello, ", "<b>", "my w", "</b>", "orld!", "</p>" };        
        private static readonly ISet<int> PlainTextInXhtml = new HashSet<int> { 1, 3, 5 };        

        // Public

        [TestMethod]
        public void PlainTextForTokenInSinglePlainTextElement()
        {
            ParsedText parsedText = CreateParsedText(Xhtml, PlainTextInXhtml);
            Token testToken = new Token
            {
                XhtmlIndex = 1,
                StringPosition = 0,
                StringLength = 5,
                TokenKind = TokenKind.Alphabetic
            };

            string expected = "Hello";
            string actual = parsedText.GetPlainText(testToken);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PlainTextForCompoundToken()
        {
            ParsedText parsedText = CreateParsedText(Xhtml, PlainTextInXhtml);
            Token testToken = new Token
            {
                XhtmlIndex = 3,
                StringPosition = 3,
                StringLength = 5
            };

            string expected = "world";
            string actual = parsedText.GetPlainText(testToken);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AllPlainText()
        {
            ParsedText parsedText = CreateParsedText(Xhtml, PlainTextInXhtml);

            string expected = "Hello, my world!";
            string actual = parsedText.GetAllPlainText();

            Assert.AreEqual(expected, actual);;
        }

        [TestMethod]
        public void PlainTextForTag()
        {
            string[] xhtml = { "<html>", "<p>", "First paragraph", "</p>", "<p>", "Second paragraph", "</p>", "</html>" };
            HashSet<int> plainTextInXhtml = new HashSet<int> { 2, 5 };
            ParsedText parsedText = CreateParsedText(xhtml, plainTextInXhtml);
            Tag[] testTags = {
                new Tag
                {
                    TagName = string.Empty,
                    TokenPosition = 0,
                    TokenLength = 3

                },
                new Tag
                {
                    TagName = string.Empty,
                    TokenPosition = 3,
                    TokenLength = 3
                }
            };
            Token[] tokens =
            {
                new Token {XhtmlIndex = 2, StringPosition = 0, StringLength = 5, TokenKind = TokenKind.Alphabetic},
                new Token {XhtmlIndex = 2, StringPosition = 5, StringLength = 1, TokenKind = TokenKind.WhiteSpace},
                new Token {XhtmlIndex = 2, StringPosition = 6, StringLength = 9, TokenKind = TokenKind.Alphabetic},
                new Token {XhtmlIndex = 5, StringPosition = 0, StringLength = 6, TokenKind = TokenKind.Alphabetic},
                new Token {XhtmlIndex = 5, StringPosition = 6, StringLength = 1, TokenKind = TokenKind.WhiteSpace},
                new Token {XhtmlIndex = 5, StringPosition = 7, StringLength = 9, TokenKind = TokenKind.Alphabetic}
            };
            foreach (Token token in tokens)
            {
                parsedText.AddToken(token);
            }

            string[] expected = { "First paragraph", "Second paragraph" };
            string[] actual = testTags.Select(tag => parsedText.GetPlainText(tag)).ToArray();

            CollectionAssert.AreEqual(expected, actual);

        }

        // Internal

        private ParsedText CreateParsedText(string[] xhtml, ISet<int> plainTextInXhtml)
        {
            var result = new ParsedText();
            result.SetXhtml(xhtml, plainTextInXhtml);            
            return result;
        }
    }
}