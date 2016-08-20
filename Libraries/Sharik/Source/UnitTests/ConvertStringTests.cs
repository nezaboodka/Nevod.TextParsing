using System;
using System.IO;
using System.Threading;
using Sharik.Text;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#else
using NUnit.Framework;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestContext = System.Object;
using TestProperty = NUnit.Framework.PropertyAttribute;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute;
using ClassInitialize = NUnit.Framework.TestFixtureSetUpAttribute;
#endif

namespace Sharik.UnitTests
{
    [TestClass]
    public class ConvertStringTests
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod, TestCategory("All"), TestCategory("Fast")]
        public void ConvertStringTest()
        {
            var rand = new Random();
            var values = new long[16];
            values[0] = 0;
            values[1] = 1;
            values[2] = 2;
            values[3] = long.MaxValue;
            values[4] = -1;
            values[5] = -2;
            values[6] = long.MinValue;
            for (var i = 7; i < values.Length; ++i)
                values[i] = (long)(long.MaxValue * (rand.NextDouble() - 0.5));
            var sample = "";
            for (var i = 0; i < values.Length; ++i)
            {
                var s = ConvertString.ToBase32String(values[i]);
                sample += string.Format("{0} -> \"{1}\"\n", values[i], s);
                Assert.IsTrue(ConvertString.FromBase32String(s) == values[i]);
            }
        }
    }
}
