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
        public void SiteExample()
        {
            string testString = "The quick(\"brown\") fox can't jump 32.3 feet, right.";
            string[] expectedResult = { "The", "quick", "brown", "fox", "can't", "jump", "32.3", "feet", "right" };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void OneWord()
        {
            string testString = "word";
            string[] expectedResult = { "word" };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void EmptyString()
        {
            string testString = "";
            string[] expectedResult = { };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void Null()
        {
            string testString = null;
            string[] expectedResult = { };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void Cyrillic()
        {
            string testString = "1А класс; 56,31 светового года, потом 45.1 дня!";
            string[] expectedResult = { "1А", "класс", "56,31", "светового", "года", "потом", "45.1", "дня" };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void OneSymbol()
        {
            string testString = "L";
            string[] expectedResult = { "L" };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void FormatCharacters()
        {
            string testString = "a\u0308b\u0308cd 3.4";
            string[] expectedResult = { "a\u0308b\u0308cd", "3.4" };
            PerformEqualityTest(testString, expectedResult);
        }

        [TestMethod]
        public void MultipleEnumeration()
        {
            string testString = "word word";
            string[] expectedResult = {"word", "word"};
            var wordExtractor = new Tokenizer();
            IEnumerable<Slice> enumerable = wordExtractor.GetTokens(testString);
            string[] firstResult = SliceEnumerableToStringArray(enumerable);
            string[] secondResult = SliceEnumerableToStringArray(enumerable);
            CollectionAssert.AreEqual(firstResult, expectedResult);
            CollectionAssert.AreEqual(secondResult, expectedResult);
        }

        // Static internals

        private static void PerformEqualityTest(string testString, string[] expectedResult)
        {
            string[] result = ExtractWords(testString);
            CollectionAssert.AreEqual(result, expectedResult);
        }

        private static string[] ExtractWords(string text)
        {
            var wordExtractor = new Tokenizer();
            string[] result = SliceEnumerableToStringArray(wordExtractor.GetTokens(text));
            return result;
        }

        private static string[] SliceEnumerableToStringArray(IEnumerable<Slice> sliceEnumerable)
        {
            return sliceEnumerable.Select(x => x.ToString()).ToArray();
        }
    }
}
