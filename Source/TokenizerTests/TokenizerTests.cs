using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Sharik.Text;

namespace TextParser.Tests
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void Latin()
        {
            string testString = "The (\"brown\") can't 32.3 feet, right?";
            Token[] expectedResult =
            {
                new Token("The".Slice(), Token.Alphabetic),
                new Token(" ".Slice(), Token.WhiteSpace),                 
                new Token("(".Slice(), Token.Symbol), 
                new Token("\"".Slice(), Token.Symbol), 
                new Token("brown".Slice(), Token.Alphabetic),
                new Token("\"".Slice(), Token.Symbol),
                new Token(")".Slice(), Token.Symbol),                
                new Token(" ".Slice(), Token.WhiteSpace),
                new Token("can".Slice(), Token.Alphabetic),
                new Token("'".Slice(), Token.Symbol),
                new Token("t".Slice(), Token.Alphabetic),
                new Token(" ".Slice(), Token.WhiteSpace),                
                new Token("32".Slice(), Token.Numeric),
                new Token(".".Slice(), Token.Symbol),
                new Token("3".Slice(), Token.Numeric),
                new Token(" ".Slice(), Token.WhiteSpace),
                new Token("feet".Slice(), Token.Alphabetic),
                new Token(",".Slice(), Token.Symbol),
                new Token(" ".Slice(), Token.WhiteSpace),
                new Token("right".Slice(), Token.Alphabetic),
                new Token("?".Slice(), Token.Symbol),
            };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void OneWord()
        {
            string testString = "word";
            Token[] expectedResult =
            {
                new Token("word".Slice(), Token.Alphabetic) 
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
        public void Null()
        {
            string testString = null;
            Token[] expectedResult = { };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void Cyrillic()
        {
            string testString = "1А класс; 56,31 светового, 45.1 дня!";
            Token[] expectedResult =
            {
                new Token("1А".Slice(), Token.Alphanumeric),
                new Token(" ".Slice(), Token.WhiteSpace),
                new Token("класс".Slice(), Token.Alphabetic),
                new Token(";".Slice(), Token.Symbol),
                new Token(" ".Slice(), Token.WhiteSpace),
                new Token("56".Slice(), Token.Numeric),
                new Token(",".Slice(), Token.Symbol),
                new Token("31".Slice(), Token.Numeric),
                new Token(" ".Slice(), Token.WhiteSpace),
                new Token("светового".Slice(), Token.Alphabetic),
                new Token(",".Slice(), Token.Symbol),
                new Token(" ".Slice(), Token.WhiteSpace),
                new Token("45".Slice(), Token.Numeric),
                new Token(".".Slice(), Token.Symbol),
                new Token("1".Slice(), Token.Numeric),
                new Token(" ".Slice(), Token.WhiteSpace),
                new Token("дня".Slice(), Token.Alphabetic),
                new Token("!".Slice(), Token.Symbol),
            };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void OneSymbol()
        {
            string testString = "L";
            Token[] expectedResult =
            {
                new Token("L".Slice(), Token.Alphabetic), 
            };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void FormatCharacters()
        {
            string testString = "a\u0308b\u0308cd 3.4";
            Token[] expectedResult =
            {
                new Token("a\u0308b\u0308cd".Slice(), Token.Symbol),
                new Token(" ".Slice(), Token.WhiteSpace), 
                new Token("3".Slice(), Token.Numeric),
                new Token(".".Slice(), Token.Symbol),
                new Token("4".Slice(), Token.Numeric), 
            };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void MultipleEnumeration()
        {
            //string testString = "word word";
            //Token[] expectedResult =
            //{
            //    new Token("word".Slice(), Token.Alphabetic),
            //    new Token(" ".Slice(), Token.WhiteSpace), 
            //    new Token("word".Slice(), Token.Alphabetic)
            //};
            //var tokenizer = new Tokenizer();
            //IEnumerable<Token> enumerable = tokenizer.GetTokensFromPlainText(testString);
            //Token[] firstResult = enumerable.ToArray();
            //Token[] secondResult = enumerable.ToArray();
            //CollectionAssert.AreEqual(firstResult, expectedResult);
            //CollectionAssert.AreEqual(secondResult, expectedResult);
            throw new NotImplementedException();
        }

        // Static internals

        private static void PerformEqualityTest(string testString, Token[] expectedResult)
        {
            Token[] result = Tokenize(testString);
            CollectionAssert.AreEqual(result, expectedResult);
        }

        private static Token[] Tokenize(string text)
        {            
            Token[] result = Tokenizer.GetTokensFromPlainText(text).ToArray();
            return result;
        }
    }
}
