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
        private static readonly Token ZeroToken = new Token()
        {
            XhtmlIndex = 0,
            StringPosition = 0,
            StringLength = 0,
            TokenKind = TokenKind.LineFeed
        };

        // Public

        public void PlainTextForTokenInSinglePlainTextElementTest()
        {
            ParsedText parsedText = CreateDefaultParsedText();
            Token testToken = new Token
            {
                XhtmlIndex = 1,
                StringPosition = 0,
                StringLength = 5,
                TokenKind = TokenKind.Alphabetic
            };
            string expected = "Hello";
            string actual = parsedText.GetTokenText(testToken);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PlainTextForCompoundTokenTest()
        {
            ParsedText parsedText = CreateDefaultParsedText();
            Token testToken = new Token
            {
                XhtmlIndex = 3,
                StringPosition = 3,
                StringLength = 5
            };
            string expected = "world";
            string actual = parsedText.GetTokenText(testToken);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AllPlainTextTest()
        {
            ParsedText parsedText = CreateDefaultParsedText();
            string expected = "Hello, my world!";
            string actual = parsedText.GetPlainText();
            Assert.AreEqual(expected, actual);;
        }

        [TestMethod]
        public void PlainTextForTagTest()
        {
            ParsedText parsedText = CreateParsedTextWithTags();
            string[] expected = { "First paragraph", "Second paragraph" };
            string[] actual = parsedText.Tags.Select(tag => parsedText.GetTagText(tag)).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AllPlainTextZeroTokensTest()
        {
            ParsedText parsedText = CreateParsedTextWithTags();
            string expected = "First paragraph\n\nSecondParagraph";
            string actual = parsedText.GetPlainText();
            Assert.AreEqual(expected, actual);
        }

        // Static internal

        private static ParsedText CreateDefaultParsedText()
        {
            string[] xhtml = { "<p>", "Hello, ", "<b>", "my w", "</b>", "orld!", "</p>" };
            ISet<int> plainTextInXhtml = new HashSet<int> { 1, 3, 5 };            
            return CreateParsedTextWithXhtml(xhtml, plainTextInXhtml);
        }

        private static ParsedText CreateParsedTextWithTags()
        {
            string[] xhtml = { "<html>", "<p>", "First paragraph", "</p>", "<p>", "Second paragraph", "</p>", "</html>" };
            HashSet<int> plainTextInXhtml = new HashSet<int> { 2, 5 };
            ParsedText parsedText = CreateParsedTextWithXhtml(xhtml, plainTextInXhtml);
            Tag[] tags = {
                new Tag
                {
                    TagName = string.Empty,
                    TokenPosition = 0,
                    TokenLength = 4

                },
                new Tag
                {
                    TagName = string.Empty,
                    TokenPosition = 4,
                    TokenLength = 4
                }
            };
            Token[] tokens =
            {
                new Token {XhtmlIndex = 2, StringPosition = 0, StringLength = 5, TokenKind = TokenKind.Alphabetic},
                new Token {XhtmlIndex = 2, StringPosition = 5, StringLength = 1, TokenKind = TokenKind.WhiteSpace},
                new Token {XhtmlIndex = 2, StringPosition = 6, StringLength = 9, TokenKind = TokenKind.Alphabetic},
                ZeroToken,
                ZeroToken,
                new Token {XhtmlIndex = 5, StringPosition = 0, StringLength = 6, TokenKind = TokenKind.Alphabetic},
                new Token {XhtmlIndex = 5, StringPosition = 6, StringLength = 1, TokenKind = TokenKind.WhiteSpace},
                new Token {XhtmlIndex = 5, StringPosition = 7, StringLength = 9, TokenKind = TokenKind.Alphabetic}
            };
            foreach (Token token in tokens)
            {
                parsedText.AddToken(token);
            }
            foreach (Tag tag in tags)
            {
                parsedText.AddTag(tag);
            }
            return parsedText;
        }

        private static ParsedText CreateParsedTextWithXhtml(string[] xhtml, ISet<int> plainTextInXhtml)
        {
            var parsedText = new ParsedText();
            parsedText.SetXhtml(xhtml, plainTextInXhtml);
            return parsedText;
        }
    }
}