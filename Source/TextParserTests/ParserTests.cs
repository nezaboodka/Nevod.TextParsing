using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TextParser.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Latin()
        {
            string testString = "The (\"brown\") can't 32.3 feet, right?";
            Tuple<string, TokenKind>[] expectedResult =
            {
                new Tuple<string, TokenKind>("The", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>(" ", TokenKind.WhiteSpace),
                new Tuple<string, TokenKind>("(", TokenKind.Symbol),
                new Tuple<string, TokenKind>("\"", TokenKind.Symbol),
                new Tuple<string, TokenKind>("brown", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>("\"", TokenKind.Symbol),
                new Tuple<string, TokenKind>(")", TokenKind.Symbol),
                new Tuple<string, TokenKind>(" ", TokenKind.WhiteSpace),
                new Tuple<string, TokenKind>("can", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>("'", TokenKind.Symbol),
                new Tuple<string, TokenKind>("t", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>(" ", TokenKind.WhiteSpace),
                new Tuple<string, TokenKind>("32", TokenKind.Numeric),
                new Tuple<string, TokenKind>(".", TokenKind.Symbol),
                new Tuple<string, TokenKind>("3", TokenKind.Numeric),
                new Tuple<string, TokenKind>(" ", TokenKind.WhiteSpace),
                new Tuple<string, TokenKind>("feet", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>(",", TokenKind.Symbol),
                new Tuple<string, TokenKind>(" ", TokenKind.WhiteSpace),
                new Tuple<string, TokenKind>("right", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>("?", TokenKind.Symbol),
            };
            ParsePlainTextAndTest(testString, expectedResult);
        }

        [TestMethod]
        public void OneWord()
        {
            string testString = "word";
            Tuple<string, TokenKind>[] expectedResult =
            {
                new Tuple<string, TokenKind>("word", TokenKind.Alphabetic)
            };
            ParsePlainTextAndTest(testString, expectedResult);
        }

        [TestMethod]
        public void Whitespaces()
        {
            string testString = "  \t";
            Tuple<string, TokenKind>[] expectedResult =
            {
                new Tuple<string, TokenKind>("  \t", TokenKind.WhiteSpace)
            };

            ParsePlainTextAndTest(testString, expectedResult);
        }


        [TestMethod]
        public void EmptyString()
        {
            string testString = "";
            Tuple<string, TokenKind>[] expectedResult = { };
            ParsePlainTextAndTest(testString, expectedResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null()
        {
            string testString = null;
            Parser.ParsePlainText(testString);
        }

        [TestMethod]
        public void Cyrillic()
        {
            string testString = "1А класс; 56,31 светового, 45.1 дня!";
            Tuple<string, TokenKind>[] expectedResult =
            {
                new Tuple<string, TokenKind>("1А", TokenKind.Alphanumeric),
                new Tuple<string, TokenKind>(" ", TokenKind.WhiteSpace),
                new Tuple<string, TokenKind>("класс", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>(";", TokenKind.Symbol),
                new Tuple<string, TokenKind>(" ", TokenKind.WhiteSpace),
                new Tuple<string, TokenKind>("56", TokenKind.Numeric),
                new Tuple<string, TokenKind>(",", TokenKind.Symbol),
                new Tuple<string, TokenKind>("31", TokenKind.Numeric),
                new Tuple<string, TokenKind>(" ", TokenKind.WhiteSpace),
                new Tuple<string, TokenKind>("светового", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>(",", TokenKind.Symbol),
                new Tuple<string, TokenKind>(" ", TokenKind.WhiteSpace),
                new Tuple<string, TokenKind>("45", TokenKind.Numeric),
                new Tuple<string, TokenKind>(".", TokenKind.Symbol),
                new Tuple<string, TokenKind>("1", TokenKind.Numeric),
                new Tuple<string, TokenKind>(" ", TokenKind.WhiteSpace),
                new Tuple<string, TokenKind>("дня", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>("!", TokenKind.Symbol),
            };
            ParsePlainTextAndTest(testString, expectedResult);
        }

        [TestMethod]
        public void OneSymbol()
        {
            string testString = "L";
            Tuple<string, TokenKind>[] expectedResult =
            {
                new Tuple<string, TokenKind>("L", TokenKind.Alphabetic),
            };
            ParsePlainTextAndTest(testString, expectedResult);
        }

        [TestMethod]
        public void FormatCharacters()
        {
            string testString = "a\u0308b\u0308cd 3.4";
            Tuple<string, TokenKind>[] expectedResult =
            {
                new Tuple<string, TokenKind>("a\u0308b\u0308cd", TokenKind.Symbol),
                new Tuple<string, TokenKind>(" ", TokenKind.WhiteSpace),
                new Tuple<string, TokenKind>("3", TokenKind.Numeric),
                new Tuple<string, TokenKind>(".", TokenKind.Symbol),
                new Tuple<string, TokenKind>("4", TokenKind.Numeric),
            };
            ParsePlainTextAndTest(testString, expectedResult);
        }

        [TestMethod]
        public void LineSeparator()
        {
            string testString = "a\nb\r\nc\rd";
            Tuple<string, TokenKind>[] expectedResult =
            {
                new Tuple<string, TokenKind>("a", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>("\n", TokenKind.LineFeed),
                new Tuple<string, TokenKind>("b", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>("\r\n", TokenKind.LineFeed),
                new Tuple<string, TokenKind>("c", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>("\r", TokenKind.LineFeed), 
                new Tuple<string, TokenKind>("d", TokenKind.Alphabetic),
            };
            ParsePlainTextAndTest(testString, expectedResult);
        }

        [TestMethod]
        public void Xhtml()
        {
            string testString = "<p>Hello, <b>w</b>orld!</p>";
            Tuple<string, TokenKind>[] expectedTokens =
            {
                new Tuple<string, TokenKind>("Hello", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>(",", TokenKind.Symbol),
                new Tuple<string, TokenKind>(" ", TokenKind.WhiteSpace),
                new Tuple<string, TokenKind>("world", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>("!", TokenKind.Symbol)
            };
            string[] expectedXhtmlElements = {"<p>", "Hello, ", "<b>", "w", "</b>", "orld!", "</p>"};
            ParseXhtmlAndTest(testString, expectedTokens, expectedXhtmlElements);
        }

        // Static internals

        private static void ParsePlainTextAndTest(string testString, Tuple<string, TokenKind>[] expectedResult)
        {
            Tuple<string, TokenKind>[] result = GetTokensFromParsedText(Parser.ParsePlainText(testString));
            CollectionAssert.AreEqual(result, expectedResult);
        }        

        private static void ParseXhtmlAndTest(string testString, Tuple<string, TokenKind>[] expectedTokens, string[] expectedXhtmlElements)
        {
            ParsedText parsedText = Parser.ParseXhtmlText(testString);
            Tuple<string, TokenKind>[] actualTokens = GetTokensFromParsedText(parsedText);
            List<string> actualXhtmlElements = parsedText.XhtmlElements;
            CollectionAssert.AreEqual(expectedTokens, actualTokens);
            CollectionAssert.AreEqual(expectedXhtmlElements, actualXhtmlElements);
        }

        private static Tuple<string, TokenKind>[] GetTokensFromParsedText(ParsedText parsedText)
        {
            return parsedText.Tokens.Select(x => new Tuple<string, TokenKind>(parsedText.GetPlainText(x), x.TokenKind)).ToArray();
        }
    }
}
