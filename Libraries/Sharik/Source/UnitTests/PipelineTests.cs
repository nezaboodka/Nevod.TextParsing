using System;
using System.IO;
using System.Linq;
using System.Threading;
using Sharik.Text;
using Sharik.Threading;

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
    public class PipelineTests
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod, TestCategory("All"), TestCategory("Fast")]
        public void PipelineTest()
        {
            var pipeline = new Pipeline<int, int>()
            {
                Hint = "TestPipeline",
                MaxWorkerCount = 100,
                DispatchTimeoutMs = 5000,
                StopOnError = false,
                SkipErrorItems = false,
                SuccessfulItemCountThreshold = 1000
            };
            var i = 1;
            var mod = 5; // processing each MOD'th item will fail with exception
            var reader = pipeline.Start(
                delegate(out int value) // infinite generator of items
                {
                    value = i++;
                    return true;
                },
                delegate(int value) // processor
                {
                    if (value % mod == 0)
                        throw new Exception(string.Format("failed item: {0}", value));
                    return value;
                });
            var results = ReaderUtil.GetEnumerable(reader).ToList(); // wait all
            Assert.IsTrue(results.TrueForAll((Pipeline<int, int>.WorkItem x) => x.Error != null || x.Result == x.Item),
                "Pipeline result items are mixed up.");
            Assert.IsTrue(results.Count((Pipeline<int, int>.WorkItem x) => x.Error == null) == pipeline.SuccessfulItemCountThreshold,
                "Unexpected number of successful items.");
            var expectedErrorCount = pipeline.SuccessfulItemCountThreshold * mod / (mod - 1) - pipeline.SuccessfulItemCountThreshold;
            Assert.IsTrue(results.Count((Pipeline<int, int>.WorkItem x) => x.Error != null) == expectedErrorCount,
                "Unexpected number of errors.");
            Assert.IsTrue(results.TrueForAll((Pipeline<int, int>.WorkItem x) => x.WorkerNumber < pipeline.SuccessfulItemCountThreshold),
                "Unexpected numbef of threads used.");
            Assert.IsTrue(results.TrueForAll((Pipeline<int, int>.WorkItem x) => x.WorkerNumber < pipeline.MaxWorkerCount),
                "Unexpected numbef of threads used.");
        }
    }
}
