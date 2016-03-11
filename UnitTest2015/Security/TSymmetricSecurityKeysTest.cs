using BLTools.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using BLTools;

//namespace BLToolsUnitTests {


//  /// <summary>
//  ///This is a test class for TSymmetricSecurityKeysTest and is intended
//  ///to contain all TSymmetricSecurityKeysTest Unit Tests
//  ///</summary>
//  [TestClass()]
//  public class TSymmetricSecurityKeysTest {


//    private TestContext testContextInstance;

//    /// <summary>
//    ///Gets or sets the test context which provides
//    ///information about and functionality for the current test run.
//    ///</summary>
//    public TestContext TestContext {
//      get {
//        return testContextInstance;
//      }
//      set {
//        testContextInstance = value;
//      }
//    }

//    #region Additional test attributes
//    // 
//    //You can use the following additional attributes as you write your tests:
//    //
//    //Use ClassInitialize to run code before running the first test in the class
//    //[ClassInitialize()]
//    //public static void MyClassInitialize(TestContext testContext)
//    //{
//    //}
//    //
//    //Use ClassCleanup to run code after all tests in a class have run
//    //[ClassCleanup()]
//    //public static void MyClassCleanup()
//    //{
//    //}
//    //
//    //Use TestInitialize to run code before running each test
//    //[TestInitialize()]
//    //public void MyTestInitialize()
//    //{
//    //}
//    //
//    //Use TestCleanup to run code after each test has run
//    //[TestCleanup()]
//    //public void MyTestCleanup()
//    //{
//    //}
//    //
//    #endregion

//    [TestMethod()]
//    public void Constructor_KeyTypeDefault_ReturnDefaultKey() {
//      TSymmetricSecurityKey actual = new TSymmetricSecurityKey();
//      TSymmetricSecurityKey expected = new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Default);
//      Assert.AreEqual(expected.SecretKey.ToHexString(), actual.SecretKey.ToHexString());
//    }

//    [TestMethod()]
//    public void Constructor_KeyTypeRandom_ReturnRandomKey() {
//      TSymmetricSecurityKey Key1 = new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Random);
//      TSymmetricSecurityKey Key2 = new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Random);
//      Trace.WriteLine(string.Format("Key1.secret={0}, Key1.IV={1}", Key1.SecretKey.ToHexString(), Key1.InitializationVector.ToHexString()));
//      Trace.WriteLine(string.Format("Key2.secret={0}, Key2.IV={1}", Key2.SecretKey.ToHexString(), Key2.InitializationVector.ToHexString()));
//      Assert.AreNotEqual(Key1.SecretKey.ToHexString(), Key2.SecretKey.ToHexString());
//    }

//    [TestMethod()]
//    [ExpectedException(typeof(ArgumentNullException))]
//    public void Constructor_KeyTypeUserKeyNull_ArgumentNullException() {
//      string SourceKey = null;
//      TSymmetricSecurityKey Key = new TSymmetricSecurityKey(SourceKey);
//    }
//  }
//}
