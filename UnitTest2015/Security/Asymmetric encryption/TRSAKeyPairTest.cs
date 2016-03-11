using BLTools.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;

namespace UnitTest2015 {


  /// <summary>
  ///This is a test class for TRSAKeyPairTest and is intended
  ///to contain all TRSAKeyPairTest Unit Tests
  ///</summary>
  [TestClass()]
  public class TRSAKeyPairTest {


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
    public void TRSAKeyPair_ConstructorEmpty_Keys1024Empty() {
      TRSAKeyPair KeyPair = new TRSAKeyPair();
      Assert.AreEqual(KeyPair.Name, string.Empty);
      Assert.AreEqual(KeyPair.KeyLength, 1024);
      Assert.AreEqual(KeyPair.KeyType, TRSAKeyPair.KeyTypeEnum.Unknown);
      Assert.AreEqual(KeyPair.PrivateKey.Key, string.Empty);
      Assert.AreEqual(KeyPair.PublicKey.Key, string.Empty);
    }

    [TestMethod(), TestCategory("RSA")]
    public void TRSAKeyPair_ConstructorKeyTypeUserPairIsBlank_Keys512Blank() {
      TRSAKeyPair KeyPair = new TRSAKeyPair("testkey", TRSAKeyPair.KeyTypeEnum.User, 512);
      Assert.AreEqual(KeyPair.KeyLength, 512);
      Assert.AreEqual(KeyPair.KeyType, TRSAKeyPair.KeyTypeEnum.User);
      Assert.AreEqual(KeyPair.PrivateKey.Key, string.Empty);
      Assert.AreEqual(KeyPair.PublicKey.Key, string.Empty);
    }

    [TestMethod(), TestCategory("RSA")]
    public void TRSAKeyPair_ConstructorKeyTypeRandom_Keys1024RandomNotEmpty() {
      TRSAKeyPair KeyPair = new TRSAKeyPair("testkey", TRSAKeyPair.KeyTypeEnum.Random, 1024);
      Assert.AreEqual(KeyPair.KeyLength, 1024);
      Assert.AreEqual(KeyPair.KeyType, TRSAKeyPair.KeyTypeEnum.Random);
      Trace.WriteLine(KeyPair.ToString());
      Assert.AreNotEqual(KeyPair.PrivateKey.Key, string.Empty);
      Assert.AreNotEqual(KeyPair.PublicKey.Key, string.Empty);
    }

    [TestMethod(), TestCategory("RSA")]
    public void TRSAKeyPair_ConstructorKeyTypeRandom_Keys1024RandomPrivateDifferent() {
      string Keyname = "testkey";
      TRSAKeyPair KeyPair1 = new TRSAKeyPair(Keyname, TRSAKeyPair.KeyTypeEnum.Random, 1024);
      TRSAKeyPair KeyPair2 = new TRSAKeyPair(Keyname, TRSAKeyPair.KeyTypeEnum.Random, 1024);
      Trace.WriteLine(KeyPair1.ToString());
      Trace.WriteLine(KeyPair2.ToString());
      Assert.AreNotEqual(KeyPair1.PrivateKey.Key, KeyPair2.PrivateKey.Key);
    }

    [TestMethod(), TestCategory("RSA")]
    public void TRSAKeyPair_ConstructorKeyTypeRandom_Keys1024RandomPublicDifferent() {
      string Keyname = "testkey";
      TRSAKeyPair KeyPair1 = new TRSAKeyPair(Keyname, TRSAKeyPair.KeyTypeEnum.Random, 1024);
      TRSAKeyPair KeyPair2 = new TRSAKeyPair(Keyname, TRSAKeyPair.KeyTypeEnum.Random, 1024);
      Trace.WriteLine(KeyPair1.ToString());
      Trace.WriteLine(KeyPair2.ToString());
      Assert.AreNotEqual(KeyPair1.PublicKey.Key, KeyPair2.PublicKey.Key);
    }

    [TestMethod(), TestCategory("RSA")]
    public void TRSAKeyPair_KeysAreSaved_FilesExist() {
      string Keyname = "testkey";
      string Pathname = Path.GetTempPath();
      
      TRSAKeyPair KeyPair1 = new TRSAKeyPair(Keyname, TRSAKeyPair.KeyTypeEnum.Random, 1024);
      KeyPair1.Save(Pathname);

      string PvtKeyfile = Path.Combine(Pathname, KeyPair1.PrivateKey.Filename);
      string PubKeyfile = Path.Combine(Pathname, KeyPair1.PublicKey.Filename);
      
      Assert.IsTrue(File.Exists(PvtKeyfile));
      Assert.IsTrue(File.Exists(PubKeyfile));

      File.Delete(PvtKeyfile);
      File.Delete(PubKeyfile);
    }

    [TestMethod(), TestCategory("RSA")]
    public void TRSAKeyPair_KeysAreSavedThenLoaded_KeysAreOK() {
      string Keyname = "testkey";
      string Pathname = Path.GetTempPath();

      TRSAKeyPair KeyPair1 = new TRSAKeyPair(Keyname, TRSAKeyPair.KeyTypeEnum.Random, 1024);
      TRSAKeyPair KeyPair2 = new TRSAKeyPair(Keyname);

      KeyPair1.Save(Pathname);
      KeyPair2.Load(Pathname);
      Assert.AreEqual(KeyPair1.PrivateKey.Key, KeyPair2.PrivateKey.Key);
      Assert.AreEqual(KeyPair1.PublicKey.Key, KeyPair2.PublicKey.Key);

      string PvtKeyfile = Path.Combine(Pathname, KeyPair1.PrivateKey.Filename);
      string PubKeyfile = Path.Combine(Pathname, KeyPair1.PublicKey.Filename);
      File.Delete(PvtKeyfile);
      File.Delete(PubKeyfile);
    }

    [TestMethod(), TestCategory("RSA")]
    public void TRSAKeyPair_KeysAreSavedThenLoadedViastatic_KeysAreOK() {
      string Keyname = "testkey";
      string Pathname = Path.GetTempPath();

      TRSAKeyPair KeyPair1 = new TRSAKeyPair(Keyname, TRSAKeyPair.KeyTypeEnum.Random, 1024);
      KeyPair1.Save(Pathname);
      TRSAKeyPair KeyPair2 = TRSAKeyPair.Load(Keyname, Pathname);

      Assert.AreEqual(KeyPair1.PrivateKey.Key, KeyPair2.PrivateKey.Key);
      Assert.AreEqual(KeyPair1.PublicKey.Key, KeyPair2.PublicKey.Key);

      string PvtKeyfile = Path.Combine(Pathname, KeyPair1.PrivateKey.Filename);
      string PubKeyfile = Path.Combine(Pathname, KeyPair1.PublicKey.Filename);
      File.Delete(PvtKeyfile);
      File.Delete(PubKeyfile);
    }
  }
}
