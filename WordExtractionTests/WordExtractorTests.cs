using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace WordExtraction.Tests
{
    [TestClass]
    public class WordExtractorTests
    {
        [TestMethod]
        public void GetWords_SiteExample()
        {
            var wordExtractor = new WordExtractor();
            string UNICODE_DEMO = "The quick(\"brown\") fox can't jump 32.3 feet, right.";
            string[] expectedResult = { "The", "quick", "brown", "fox", "can't", "jump", "32.3", "feet", "right" };

            string[] result = wordExtractor.GetWords(UNICODE_DEMO).ToArray();

            CollectionAssert.AreEqual(result, expectedResult);
        }
    }
}