using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace WordExtraction.Tests
{
    [TestClass]
    public class WordExtractorTests
    {
        private string[] ExtractWords(string text)
        {
            var wordExtractor = new WordExtractor();
            string[] result = wordExtractor.GetWords(text).ToArray();
            return result;
        }

        private void PerformTest(string testString, string[] expectedResult)
        {
            string[] result = ExtractWords(testString);
            CollectionAssert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void SiteExample()
        {
            string testString = "The quick(\"brown\") fox can't jump 32.3 feet, right.";
            string[] expectedResult = { "The", "quick", "brown", "fox", "can't", "jump", "32.3", "feet", "right" };

            PerformTest(testString, expectedResult);
        }

        [TestMethod]
        public void OneWord()
        {
            string testString = "word";
            string[] expectedResult = { "word" };

            PerformTest(testString, expectedResult);
        }

        [TestMethod]
        public void EmptyString()
        {
            string testString = "";
            string[] expectedResult = {};

            PerformTest(testString, expectedResult);
        }

        [TestMethod]
        public void Null()
        {
            string testString = null;
            string[] expectedResult = {};

            PerformTest(testString, expectedResult);
        }

        [TestMethod]
        public void Сyrillic()
        {
            string testString = "1А класс; 56,31 световых года, потом 100500 45.10 дней!";
            string[] expectedResult = { "1А", "класс", "56,31", "световых", "года", "потом", "100500", "45.10", "дней" };

            PerformTest(testString, expectedResult);
        }
    }
}