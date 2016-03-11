using BLTools.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using BLTools;

//namespace BLToolsUnitTests
//{
    
    
//    /// <summary>
//    ///This is a test class for TSymmetricSecurityExtensionTest and is intended
//    ///to contain all TSymmetricSecurityExtensionTest Unit Tests
//    ///</summary>
//  [TestClass()]
//  public class TSymmetricSecurityExtensionTest {


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

//    #region >> string encryption/decryption <<
//    #region EncryptTo3DESBase64
//    [TestMethod()]
//    public void EncryptTo3DESBase64_StandardStringKeyTypeDefault_ReturnEncryptedString() {
//      string source = "Here's a source string";
//      string expected = "ZUAVvLM8j4jyWyFmldrz37U1gVr7UNTQ";
//      string actual = source.EncryptTo3DESBase64(TSymmetricSecurityKey.KeyType.Default);
//      Assert.AreEqual(expected, actual);
//    }

//    [TestMethod()]
//    public void EncryptTo3DESBase64_StandardStringKeyTypeRandom_ReturnEncryptedString() {
//      string source = "Here's a source string";
//      TSymmetricSecurityKey SKey = new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Random);
//      Trace.WriteLine(SKey.ToString());
//      string actual = source.EncryptTo3DESBase64(SKey);
//      Assert.AreEqual(source, actual.DecryptFrom3DESBase64(SKey));
//    }

//    [TestMethod()]
//    [ExpectedException(typeof(ArgumentNullException))]
//    public void EncryptTo3DESBase64_NullStringKeyTypeDefault_ArgumentNullException() {
//      string source = null;
//      string actual = source.EncryptTo3DESBase64();
//    }

//    [TestMethod()]
//    public void EncryptTo3DESBase64_BlankStringKeyTypeDefault_ReturnEncryptedBlankString() {
//      string source = "";
//      string actual = source.EncryptTo3DESBase64();
//      Assert.AreEqual(source, actual.DecryptFrom3DESBase64());
//    }

//    [TestMethod()]
//    [ExpectedException(typeof(ArgumentNullException))]
//    public void EncryptTo3DESBase64_StandardStringKeyTypeNull_ArgumentNullException() {
//      string source = "Here's a source string";
//      string actual = source.EncryptTo3DESBase64(null);
//    }
//    #endregion EncryptTo3DESBase64

//    #region DecryptFrom3DESBase64
//    [TestMethod()]
//    public void DecryptFrom3DESBase64_StandardStringKeyTypeDefault_ReturnDecryptedString() {
//      string source = "ZUAVvLM8j4jyWyFmldrz37U1gVr7UNTQ";
//      string expected = "Here's a source string";
//      string actual = source.DecryptFrom3DESBase64();
//      Assert.AreEqual(expected, actual);
//    }

//    [TestMethod()]
//    public void DecryptFrom3DESBase64_StandardStringKeyTypeRandom_ReturnDecryptedString() {
//      string source = "Here's a source string";
//      TSymmetricSecurityKey SKey = new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Random);
//      Trace.WriteLine(SKey.ToString());
//      string actual = source.EncryptTo3DESBase64(SKey);
//      Assert.AreEqual(source, actual.DecryptFrom3DESBase64(SKey));
//    }

//    [TestMethod()]
//    [ExpectedException(typeof(ArgumentNullException))]
//    public void DecryptFrom3DESBase64_NullStringKeyTypeDefault_ArgumentNullException() {
//      string source = null;
//      string expected = "";
//      string actual = source.DecryptFrom3DESBase64();
//      Assert.AreEqual(expected, actual);
//    }

//    [TestMethod()]
//    public void DecryptFrom3DESBase64_BlankStringKeyTypeDefault_ReturnDecryptedBlankString() {
//      string source = "";
//      string expected = "";
//      string actual = source.DecryptFrom3DESBase64();
//      Assert.AreEqual(expected, actual);
//    }

//    [TestMethod()]
//    [ExpectedException(typeof(ArgumentNullException))]
//    public void DecryptFrom3DESBase64_StandardStringKeyTypeNull_ArgumentNullException() {
//      string source = "Here's a source string";
//      string actual = source.DecryptFrom3DESBase64(null);
//    }
//    #endregion DecryptFrom3DESBase64

//    #region Encrypt/Decrypt
//    [TestMethod()]
//    public void EncryptThenDecryptTo3DESBase64_StandardStringKeyTypeRandom_ReturnDecryptedString() {
//      string source = "Here's a source string";
//      string expected = "Here's a source string";
//      TSymmetricSecurityKey Key = new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Random);
//      string actual = source.EncryptTo3DESBase64(Key).DecryptFrom3DESBase64(Key);
//      Assert.AreEqual(expected, actual);
//    }
//    #endregion Encrypt/Decrypt

//    #region EncryptStringToFile
//    [TestMethod()]
//    public void EncryptStringToFileTest_StandardStringSecurityKeyDefault_MakeEncryptedFile() {
//      string source = "Here's a source string";
//      string Expected = "Here's a source string";
//      string fileName = Path.Combine(Path.GetTempPath(), "testfile.bin");
//      source.EncryptStringToFile(fileName);
//      Assert.AreEqual(Expected, fileName.DecryptFileToString());
//      File.Delete(fileName);
//    }

//    [TestMethod()]
//    public void EncryptStringToFileTest_BlankStringSecurityKeyDefault_MakeEncryptedFile() {
//      string source = "";
//      string Expected = "";
//      string fileName = Path.Combine(Path.GetTempPath(), "testfile.bin");
//      source.EncryptStringToFile(fileName);
//      Assert.AreEqual(Expected, fileName.DecryptFileToString());
//      File.Delete(fileName);
//    }

//    [TestMethod()]
//    [ExpectedException(typeof(ArgumentNullException))]
//    public void EncryptStringToFileTest_NullStringSecurityKeyDefault_ArgumentNullException() {
//      string source = null;
//      string fileName = Path.Combine(Path.GetTempPath(), "testfile.bin");
//      source.EncryptStringToFile(fileName);
//    }

//    [TestMethod()]
//    [ExpectedException(typeof(System.ArgumentNullException))]
//    public void EncryptStringToFileTest_StandardStringSecurityKeyDefaultNullFilename_ArgumentNullException() {
//      string source = "Here's a source string";
//      string fileName = null;
//      source.EncryptStringToFile(fileName);
//    }

//    [TestMethod()]
//    [ExpectedException(typeof(System.ArgumentException))]
//    public void EncryptStringToFileTest_StandardStringSecurityKeyDefaultNullFilename_ArgumentException() {
//      string source = "Here's a source string";
//      string fileName = "";
//      source.EncryptStringToFile(fileName);
//    }
//    #endregion EncryptStringToFile

//    #region DecryptFileToString
//    [TestMethod()]
//    public void DecryptFileToStringTest_EncryptedStandardStringSecurityKeyDefault_ReturnDecryptedString() {
//      string fileName = Path.Combine(Path.GetTempPath(), "testfile.bin");
//      string Source = "Here's a source string";
//      Source.EncryptStringToFile(fileName);
//      string Expected = "Here's a source string";
//      string Actual = fileName.DecryptFileToString();
//      Assert.AreEqual(Expected, Actual);
//      File.Delete(fileName);
//    }

//    [TestMethod()]
//    public void DecryptFileToStringTest_EncryptedBlankStringSecurityKeyDefault_ReturnBlankString() {
//      string fileName = Path.Combine(Path.GetTempPath(), "testfile.bin");
//      string Source = "";
//      Source.EncryptStringToFile(fileName);
//      string Expected = "";
//      string Actual = fileName.DecryptFileToString();
//      Assert.AreEqual(Expected, Actual);
//      File.Delete(fileName);
//    }

//    [TestMethod()]
//    [ExpectedException(typeof(System.ArgumentException))]
//    public void DecryptFileToStringTest_StandardStringSecurityKeyDefaultBlankFilename_ArgumentException() {
//      string fileName = "";
//      string Expected = fileName.DecryptFileToString();
//    }

//    [TestMethod()]
//    [ExpectedException(typeof(System.ArgumentNullException))]
//    public void DecryptFileToStringTest_StandardStringSecurityKeyDefaultNullFilename_ArgumentNullException() {
//      string fileName = null;
//      string Expected = fileName.DecryptFileToString();
//    }

//    [TestMethod()]
//    public void DecryptFileToStringTest_EncryptStringToFileSecurityKeyRandom_ReturnDecryptedString() {
//      string fileName = Path.Combine(Path.GetTempPath(), "testfile.bin");
//      string Source = "Here's a source string";
//      TSymmetricSecurityKey Key = new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Random);
//      Source.EncryptStringToFile(fileName, Key);
//      string Expected = "Here's a source string";
//      string Actual = fileName.DecryptFileToString(Key);
//      Assert.AreEqual(Expected, Actual);
//      File.Delete(fileName);
//    }
//    #endregion DecryptFileToString 
//    #endregion >> string encryption/decryption <<

//    #region >> Byte[] encryption/decryption <<

//    #region EncryptTo3DESBase64
//    [TestMethod()]
//    public void EncryptTo3DESBase64_StandardBytesKeyTypeDefault_ReturnEncryptedBase64String() {
//      byte[] source = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
//      string actual = source.EncryptTo3DESBase64();
//      Assert.AreEqual(source.ToHexString(), actual.DecryptFrom3DESBase64ToBytes().ToHexString());
//    }

//    [TestMethod()]
//    public void EncryptTo3DESBase64_StandardBytesKeyTypeRandom_ReturnEncryptedBase64String() {
//      byte[] source = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
//      TSymmetricSecurityKey Key = new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Random);
//      string actual = source.EncryptTo3DESBase64(Key);
//      Assert.AreEqual(source.ToHexString(), actual.DecryptFrom3DESBase64ToBytes(Key).ToHexString());
//    }

//    [TestMethod()]
//    [ExpectedException(typeof(ArgumentNullException))]
//    public void EncryptTo3DESBase64_NullBytesKeyTypeDefault_ArgumentNullException() {
//      byte[] source = null;
//      string actual = source.EncryptTo3DESBase64();
//    }

//    [TestMethod()]
//    public void EncryptTo3DESBase64_EmptyBytesKeyTypeDefault_ReturnEncryptedBlankString() {
//      byte[] source = new byte[0];
//      string actual = source.EncryptTo3DESBase64();
//      Assert.AreEqual(source.ToHexString(), actual.DecryptFrom3DESBase64ToBytes().ToHexString());
//    }

//    [TestMethod()]
//    [ExpectedException(typeof(ArgumentNullException))]
//    public void EncryptTo3DESBase64_StandardBytesKeyTypeNull_ArgumentNullException() {
//      byte[] source = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
//      string actual = source.EncryptTo3DESBase64(null);
//    }
//    #endregion EncryptTo3DESBase64

//    //#region DecryptFrom3DESBase64
//    //[TestMethod()]
//    //public void DecryptFrom3DESBase64_StandardStringKeyTypePwd_ReturnDecryptedBytes() {
//    //  string source = "TVSFTYwuZzjqHkSVcJMIWA==";
//    //  byte[] expected = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
//    //  byte[] actual = source.DecryptFrom3DESBase64ToBytes(new SecurityKeys(SecurityKeys.KeyType.Pwd));
//    //  CollectionAssert.AreEqual(expected, actual);
//    //}

//    //[TestMethod()]
//    //public void DecryptFrom3DESBase64_StandardStringKeyTypeSql_ReturnDecryptedBytes() {
//    //  string source = "RWNsM7PzTinbds69gLObgg==";
//    //  byte[] expected = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
//    //  byte[] actual = source.DecryptFrom3DESBase64ToBytes(new SecurityKeys(SecurityKeys.KeyType.Sql));
//    //  CollectionAssert.AreEqual(expected, actual);
//    //}

//    //[TestMethod()]
//    //[ExpectedException(typeof(ArgumentException))]
//    //public void DecryptFrom3DESBase64_NullStringKeyTypePwd_ArgumentException() {
//    //  string source = null;
//    //  byte[] expected = new byte[0];
//    //  byte[] actual = source.DecryptFrom3DESBase64ToBytes(new SecurityKeys(SecurityKeys.KeyType.Pwd));
//    //}

//    //[TestMethod()]
//    //public void DecryptFrom3DESBase64_BlankStringKeyTypePwd_ReturnEmptyBytes() {
//    //  string source = "";
//    //  byte[] expected = new byte[0];
//    //  byte[] actual = source.DecryptFrom3DESBase64ToBytes(new SecurityKeys(SecurityKeys.KeyType.Pwd));
//    //  CollectionAssert.AreEqual(expected, actual);
//    //}

//    //[TestMethod()]
//    //public void DecryptFrom3DESBase64_StandardStringKeyTypeNull_ReturnNull() {
//    //  string source = "TVSFTYwuZzjqHkSVcJMIWA==";
//    //  byte[] expected = null;
//    //  byte[] actual = source.DecryptFrom3DESBase64ToBytes(null);
//    //  CollectionAssert.AreEqual(expected, actual);
//    //}
//    //#endregion DecryptFrom3DESBase64

//    //#region Encrypt/Decrypt
//    //[TestMethod()]
//    //public void EncryptThenDecryptTo3DESBase64_StandardBytesKeyTypePwd_ReturnDecryptedBytes() {
//    //  byte[] source = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
//    //  byte[] expected = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
//    //  SecurityKeys Keys = new SecurityKeys(SecurityKeys.KeyType.Pwd);
//    //  byte[] actual = source.EncryptTo3DESBase64(Keys).DecryptFrom3DESBase64ToBytes(Keys);
//    //  CollectionAssert.AreEqual(expected, actual);
//    //}

//    //[TestMethod()]
//    //public void EncryptThenDecryptToFile_StandardBytesSecurityKeyPwd_MakeEncryptedFile() {
//    //  byte[] source = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
//    //  byte[] expected = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
//    //  string fileName = Path.Combine(Path.GetTempPath(), "testfile.txt");
//    //  source.EncryptBytesToFile(fileName, SecurityKeys.KeyType.Pwd);
//    //  CollectionAssert.AreEqual(expected, fileName.DecryptFileToBytes(SecurityKeys.KeyType.Pwd));
//    //  File.Delete(fileName);
//    //}
//    //#endregion Encrypt/Decrypt

//    #endregion >> Byte[] encryption/decryption <<
//  }
//}
