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
    public class TokenizerResultTests
    {
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void AddTokenWithNegativePosition()
        {
            TokenizerResult tokenizerResult = new TokenizerResult(string.Empty);

            tokenizerResult.AddToken(-1, 0);
            
            // Assert - IndexOutOfRangeException
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void AddTokenWithTooBigPosition()
        {
            string s = string.Empty;
            TokenizerResult tokenizerResult = new TokenizerResult(s);

            tokenizerResult.AddToken(s.Length, 0);

            // Assert - IndexOutOfRangeException
        }

        [TestMethod]
        public void TokenEnumeration()
        {
            string s = "hello, world";
            TokenizerResult tokenizerResult = new TokenizerResult(s);

            tokenizerResult.AddToken(s.IndexOf("hello") + "hello".Length - 1, 0);
            tokenizerResult.AddToken(s.IndexOf(","), 1);
            tokenizerResult.AddToken(s.IndexOf(" "), 2);
            tokenizerResult.AddToken(s.IndexOf("world") + "world".Length - 1, 3);
                        
            Token[] expected =
            {
                new Token("hello".Slice(), 0),
                new Token(",".Slice(), 1),
                new Token(" ".Slice(), 2),
                new Token("world".Slice(), 3)
            };

            CollectionAssert.AreEqual(expected, tokenizerResult.ToArray());
        }
    }
}