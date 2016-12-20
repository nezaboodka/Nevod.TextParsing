using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
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
        public void TokenInSinglePlainTextElement()
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
        public void CompoundToken()
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
        public void PlainText()
        {
            ParsedText parsedText = CreateParsedText(Xhtml, PlainTextInXhtml);

            string expected = "Hello, my world!";
            string actual = parsedText.GetAllPlainText();

            Assert.AreEqual(expected, actual);;
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