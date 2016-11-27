using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharik.Text;

namespace TextParser.Tests
{
    [TestClass]
    public class ParsedTextTests
    {
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void AddTokenWithNegativePosition()
        {
            ParsedText parsedText = new ParsedText(string.Empty);

            parsedText.AddToken(-1, 0);
            
            // Assert - IndexOutOfRangeException
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void AddTokenWithTooBigPosition()
        {
            string s = string.Empty;
            ParsedText parsedText = new ParsedText(s);

            parsedText.AddToken(s.Length, 0);

            // Assert - IndexOutOfRangeException
        }

        [TestMethod]
        public void TokenEnumeration()
        {
            string s = "hello, world";
            ParsedText parsedText = new ParsedText(s);

            parsedText.AddToken(s.IndexOf("hello") + "hello".Length - 1, TokenKind.Alphabetic);
            parsedText.AddToken(s.IndexOf(","), TokenKind.Symbol);
            parsedText.AddToken(s.IndexOf(" "), TokenKind.WhiteSpace);
            parsedText.AddToken(s.IndexOf("world") + "world".Length - 1, TokenKind.Alphabetic);

            Token[] expected =
            {
                new Token("hello".Slice(), TokenKind.Alphabetic),
                new Token(",".Slice(), TokenKind.Symbol),
                new Token(" ".Slice(), TokenKind.WhiteSpace),
                new Token("world".Slice(), TokenKind.Alphabetic)
            };

            CollectionAssert.AreEqual(expected, parsedText.Tokens.ToArray());
        }
    }
}