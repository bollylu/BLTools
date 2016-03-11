using BLTools.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;
using System.IO;

namespace UnitTest2015 {


  /// <summary>
  ///This is a test class for TRSAPublicKeyTest and is intended
  ///to contain all TRSAPublicKeyTest Unit Tests
  ///</summary>
  [TestClass()]
  public class TRSAPublicKeyTest {


    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext {
      get {
        return testContextInstance;
      }
      set {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    [TestMethod(), TestCategory("RSA")]
    public void TRSAPublicKey_ConstructorEmpty_KeyEmpty() {
      TRSAPublicKey target = new TRSAPublicKey();
      Assert.AreEqual(target.Key, string.Empty);
    }

    [TestMethod(), TestCategory("RSA")]
    public void TRSAPublicKey_ConstructorValidString_KeyValidString() {
      string key = "0263489516513216541";
      TRSAPublicKey target = new TRSAPublicKey("testkey", key);
      Assert.AreEqual(target.Key, key);
    }

    /// <summary>
    ///A test for Save
    ///</summary>
    [TestMethod(), TestCategory("RSA")]
    public void TRSAPublicKey_SaveKey_KeyIsSaved() {
      string Key = "0263489516513216541";
      TRSAPublicKey Target = new TRSAPublicKey("testkey", Key);
      string Pathname = Path.GetTempPath();
      string Keyfile = Path.Combine(Pathname, Target.Filename);
      Target.Save(Pathname);
      Assert.IsTrue(File.Exists(Keyfile));
      File.Delete(Keyfile);
    }

    /// <summary>
    ///A test for Load
    ///</summary>
    [TestMethod(), TestCategory("RSA")]
    public void TRSAPublicKey_KeyIsSavedThenLoaded_KeyIsOK() {
      string Key = "0263489516513216541";
      TRSAPublicKey Source = new TRSAPublicKey("testkey", Key);
      string Pathname = Path.GetTempPath();
      Source.Save(Pathname);
      TRSAPublicKey Target = new TRSAPublicKey("testkey");
      Target.Load(Pathname);
      Assert.AreEqual(Source.Key, Target.Key);
      string Keyfile = Path.Combine(Pathname, Source.Filename);
      File.Delete(Keyfile);
    }

    /// <summary>
    ///A test for Load
    ///</summary>
    [TestMethod(), TestCategory("RSA")]
    public void TRSAPublicKey_KeyIsLoadedViaStatic_KeyIsOK() {
      string Key = "0263489516513216541";
      TRSAPublicKey Source = new TRSAPublicKey("testkey", Key);
      string Pathname = Path.GetTempPath();
      Source.Save(Pathname);

      TRSAPublicKey Target = TRSAPublicKey.Load("testkey", Pathname);

      Assert.AreEqual(Source.Key, Target.Key);
      string Keyfile = Path.Combine(Pathname, Source.Filename);
      File.Delete(Keyfile);
    }
  }
}
