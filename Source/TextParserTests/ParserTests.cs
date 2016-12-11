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
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void OneWord()
        {
            string testString = "word";
            Tuple<string, TokenKind>[] expectedResult =
            {
                new Tuple<string, TokenKind>("word", TokenKind.Alphabetic)
            };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void Whitespaces()
        {
            string testString = "  \t";
            Tuple<string, TokenKind>[] expectedResult =
            {
                new Tuple<string, TokenKind>("  \t", TokenKind.WhiteSpace)
            };

            PerformEqualityTest(testString, expectedResult);
        }


        [TestMethod]
        public void EmptyString()
        {
            string testString = "";
            Tuple<string, TokenKind>[] expectedResult = { };
            PerformEqualityTest(testString, expectedResult);
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
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void OneSymbol()
        {
            string testString = "L";
            Tuple<string, TokenKind>[] expectedResult =
            {
                new Tuple<string, TokenKind>("L", TokenKind.Alphabetic),
            };
            PerformEqualityTest(testString, expectedResult);
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
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void LineSeparator()
        {
            string testString = "a\nb\r\nc\rd";
            Tuple<string, TokenKind>[] expectedResult =
            {
                new Tuple<string, TokenKind>("a", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>("\n", TokenKind.LineSeparator),
                new Tuple<string, TokenKind>("b", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>("\r\n", TokenKind.LineSeparator),
                new Tuple<string, TokenKind>("c", TokenKind.Alphabetic),
                new Tuple<string, TokenKind>("\r", TokenKind.LineSeparator), 
                new Tuple<string, TokenKind>("d", TokenKind.Alphabetic),
            };
            PerformEqualityTest(testString, expectedResult);
        }

        // Static internals

        private static void PerformEqualityTest(string testString, Tuple<string, TokenKind>[] expectedResult)
        {
            Tuple<string, TokenKind>[] result = Tokenize(testString);
            CollectionAssert.AreEqual(result, expectedResult);
        }

        private static Tuple<string, TokenKind>[] Tokenize(string text)
        {
            var parsedText = Parser.ParsePlainText(text);
            Tuple<string, TokenKind>[] result = parsedText.Tokens.Select(x => new Tuple<string, TokenKind>(parsedText.GetPlainText(x), x.TokenKind)).ToArray();
            return result;
        }
    }
}
