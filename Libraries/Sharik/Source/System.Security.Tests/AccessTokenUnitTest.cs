using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharik.Security;

namespace System.Security.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class AccessTokenUnitTest
    {
        public AccessTokenUnitTest()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestAccessTokenCreation()
        {
            var t = AccessToken.Create(TimeSpan.FromMinutes(60 * 8), Guid.NewGuid().ToString("B"),
                Guid.NewGuid().ToString("B"), "dmitry.surkov@gmail.com", null);
            var ts = t.ToString();
            var t2 = AccessToken.CreateFromString(ts);
            Assert.AreEqual(t.ValidFrom, t2.ValidFrom);
            Assert.AreEqual(t.ValidTo, t2.ValidTo);
            Assert.AreEqual(t.TokenGuid, t2.TokenGuid);
            Assert.AreEqual(t.Customer, t2.Customer);
            Assert.AreEqual(t.Account, t2.Account);
            Assert.AreEqual(t.User, t2.User);
            Assert.IsTrue(t.ResourcePermissions.Count == 0);
            Assert.IsTrue(t2.ResourcePermissions.Count == 0);
        }

        [TestMethod]
        public void TestAccessTokenEncryptionDecryption()
        {
            // Create and encrypt access token
            var permission = new ResourcePermission();
            permission.Service = "FS";
            permission.Resource = "bucket";
            permission.Permission = "RW";
            var permissions = new List<ResourcePermission>(1);
            permissions.Add(permission);
            var t = AccessToken.Create(TimeSpan.FromMinutes(60 * 8), Guid.NewGuid().ToString("B"),
                Guid.NewGuid().ToString("B"), "dmitry.surkov@gmail.com", permissions);
            var alg = new RijndaelManaged();
            alg.KeySize = 256;
            var ts = t.ToEncryptedString(alg.Key, alg.IV);
            // Decrypt and parse access token
            AccessToken t2 = AccessToken.TryCreateFromEncryptedString(ts, alg.Key, alg.IV);
            Assert.IsNotNull(t2);
            // Initial and resulting tokens should match
            Assert.AreEqual(t.ValidFrom, t2.ValidFrom);
            Assert.AreEqual(t.ValidTo, t2.ValidTo);
            Assert.AreEqual(t.TokenGuid, t2.TokenGuid);
            Assert.AreEqual(t.Customer, t2.Customer);
            Assert.AreEqual(t.Account, t2.Account);
            Assert.AreEqual(t.User, t2.User);
            Assert.IsTrue(t.ResourcePermissions.Count == t2.ResourcePermissions.Count);
            Assert.AreEqual(t.ResourcePermissions[0].Service, t2.ResourcePermissions[0].Service);
            Assert.AreEqual(t.ResourcePermissions[0].Resource, t2.ResourcePermissions[0].Resource);
            Assert.AreEqual(t.ResourcePermissions[0].Permission, t2.ResourcePermissions[0].Permission);
        }
    }
}
