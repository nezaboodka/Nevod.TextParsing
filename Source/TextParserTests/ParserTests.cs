using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Sharik.Text;

namespace TextParser.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Latin()
        {
            string testString = "The (\"brown\") can't 32.3 feet, right?";
            Token[] expectedResult =
            {
                new Token("The".Slice(), TokenKind.Alphabetic),
                new Token(" ".Slice(), TokenKind.WhiteSpace),                 
                new Token("(".Slice(), TokenKind.Symbol), 
                new Token("\"".Slice(), TokenKind.Symbol), 
                new Token("brown".Slice(), TokenKind.Alphabetic),
                new Token("\"".Slice(), TokenKind.Symbol),
                new Token(")".Slice(), TokenKind.Symbol),                
                new Token(" ".Slice(), TokenKind.WhiteSpace),
                new Token("can".Slice(), TokenKind.Alphabetic),
                new Token("'".Slice(), TokenKind.Symbol),
                new Token("t".Slice(), TokenKind.Alphabetic),
                new Token(" ".Slice(), TokenKind.WhiteSpace),                
                new Token("32".Slice(), TokenKind.Numeric),
                new Token(".".Slice(), TokenKind.Symbol),
                new Token("3".Slice(), TokenKind.Numeric),
                new Token(" ".Slice(), TokenKind.WhiteSpace),
                new Token("feet".Slice(), TokenKind.Alphabetic),
                new Token(",".Slice(), TokenKind.Symbol),
                new Token(" ".Slice(), TokenKind.WhiteSpace),
                new Token("right".Slice(), TokenKind.Alphabetic),
                new Token("?".Slice(), TokenKind.Symbol),
            };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void OneWord()
        {
            string testString = "word";
            Token[] expectedResult =
            {
                new Token(testString.Slice(), TokenKind.Alphabetic) 
            };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void Whitespaces()
        {
            string testString = "  \t";
            Token[] expectedResult =
            {
                new Token(testString.Slice(), TokenKind.WhiteSpace)
            };

            PerformEqualityTest(testString, expectedResult);
        }


        [TestMethod]
        public void EmptyString()
        {
            string testString = "";
            Token[] expectedResult = { };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null()
        {
            string testString = null;
            Token[] expectedResult = { };
            PerformEqualityTest(testString, expectedResult);

            // Assert - ArgumentNullException
        }

        [TestMethod]
        public void Cyrillic()
        {
            string testString = "1А класс; 56,31 светового, 45.1 дня!";
            Token[] expectedResult =
            {
                new Token("1А".Slice(), TokenKind.Alphanumeric),
                new Token(" ".Slice(), TokenKind.WhiteSpace),
                new Token("класс".Slice(), TokenKind.Alphabetic),
                new Token(";".Slice(), TokenKind.Symbol),
                new Token(" ".Slice(), TokenKind.WhiteSpace),
                new Token("56".Slice(), TokenKind.Numeric),
                new Token(",".Slice(), TokenKind.Symbol),
                new Token("31".Slice(), TokenKind.Numeric),
                new Token(" ".Slice(), TokenKind.WhiteSpace),
                new Token("светового".Slice(), TokenKind.Alphabetic),
                new Token(",".Slice(), TokenKind.Symbol),
                new Token(" ".Slice(), TokenKind.WhiteSpace),
                new Token("45".Slice(), TokenKind.Numeric),
                new Token(".".Slice(), TokenKind.Symbol),
                new Token("1".Slice(), TokenKind.Numeric),
                new Token(" ".Slice(), TokenKind.WhiteSpace),
                new Token("дня".Slice(), TokenKind.Alphabetic),
                new Token("!".Slice(), TokenKind.Symbol),
            };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void OneSymbol()
        {
            string testString = "L";
            Token[] expectedResult =
            {
                new Token("L".Slice(), TokenKind.Alphabetic), 
            };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void FormatCharacters()
        {
            string testString = "a\u0308b\u0308cd 3.4";
            Token[] expectedResult =
            {
                new Token("a\u0308b\u0308cd".Slice(), TokenKind.Symbol),
                new Token(" ".Slice(), TokenKind.WhiteSpace), 
                new Token("3".Slice(), TokenKind.Numeric),
                new Token(".".Slice(), TokenKind.Symbol),
                new Token("4".Slice(), TokenKind.Numeric), 
            };
            PerformEqualityTest(testString, expectedResult);
        }

        // Static internals

        private static void PerformEqualityTest(string testString, Token[] expectedResult)
        {
            Token[] result = Tokenize(testString);
            CollectionAssert.AreEqual(result, expectedResult);
        }

        private static Token[] Tokenize(string text)
        {            
            Token[] result = Parser.GetTokensFromPlainText(text).ToArray();
            return result;
        }
    }
}
