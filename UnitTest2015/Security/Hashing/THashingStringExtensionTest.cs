using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLTools.Encryption;

namespace UnitTest2015 {
  [TestClass]
  public class THashingStringExtensionTest {
    #region MD5
    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("MD5")]
    public void TestHashMD5_StandardString_HashIsOK() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.MD5);
      Assert.IsTrue(SourceString.VerifyHashFromBase64(Base64Hash, THashingMethods.MD5));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("MD5")]
    public void TestHashMD5_StandardStringModifiedForVerify_VerifyIsFalse() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.MD5);
      string TestString = SourceString + "0";
      Assert.IsFalse(TestString.VerifyHashFromBase64(Base64Hash, THashingMethods.MD5));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("MD5")]
    public void TestHashMD5_StandardStringHashModified_VerifyIsFalse() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.MD5);
      string TestHash = Base64Hash + "0";
      Assert.IsFalse(SourceString.VerifyHashFromBase64(TestHash, THashingMethods.MD5));
    } 
    #endregion MD5

    #region SHA1
    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA1")]
    public void TestHashSHA1_StandardString_HashIsOK() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.SHA1);
      Assert.IsTrue(SourceString.VerifyHashFromBase64(Base64Hash, THashingMethods.SHA1));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA1")]
    public void TestHashSHA1_StandardStringModifiedForVerify_VerifyIsFalse() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.SHA1);
      string TestString = SourceString + "0";
      Assert.IsFalse(TestString.VerifyHashFromBase64(Base64Hash, THashingMethods.SHA1));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA1")]
    public void TestHashSHA1_StandardStringHashModified_VerifyIsFalse() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.SHA1);
      string TestHash = Base64Hash + "0";
      Assert.IsFalse(SourceString.VerifyHashFromBase64(TestHash, THashingMethods.SHA1));
    }  
    #endregion SHA1

    #region SHA256
    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA256")]
    public void TestHashSHA256_StandardString_HashIsOK() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.SHA256);
      Assert.IsTrue(SourceString.VerifyHashFromBase64(Base64Hash, THashingMethods.SHA256));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA256")]
    public void TestHashSHA256_StandardStringModifiedForVerify_VerifyIsFalse() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.SHA256);
      string TestString = SourceString + "0";
      Assert.IsFalse(TestString.VerifyHashFromBase64(Base64Hash, THashingMethods.SHA256));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA256")]
    public void TestHashSHA256_StandardStringHashModified_VerifyIsFalse() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.SHA256);
      string TestHash = Base64Hash + "0";
      Assert.IsFalse(SourceString.VerifyHashFromBase64(TestHash, THashingMethods.SHA256));
    } 
    #endregion SHA256 

    #region SHA384
    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA384")]
    public void TestHashSHA384_StandardString_HashIsOK() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.SHA384);
      Assert.IsTrue(SourceString.VerifyHashFromBase64(Base64Hash, THashingMethods.SHA384));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA384")]
    public void TestHashSHA384_StandardStringModifiedForVerify_VerifyIsFalse() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.SHA384);
      string TestString = SourceString + "0";
      Assert.IsFalse(TestString.VerifyHashFromBase64(Base64Hash, THashingMethods.SHA384));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA384")]
    public void TestHashSHA384_StandardStringHashModified_VerifyIsFalse() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.SHA384);
      string TestHash = Base64Hash + "0";
      Assert.IsFalse(SourceString.VerifyHashFromBase64(TestHash, THashingMethods.SHA384));
    }  
    #endregion SHA384

    #region SHA512
    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA512")]
    public void TestHashSHA512_StandardString_HashIsOK() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.SHA512);
      Assert.IsTrue(SourceString.VerifyHashFromBase64(Base64Hash, THashingMethods.SHA512));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA512")]
    public void TestHashSHA512_StandardStringModifiedForVerify_VerifyIsFalse() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.SHA512);
      string TestString = SourceString + "0";
      Assert.IsFalse(TestString.VerifyHashFromBase64(Base64Hash, THashingMethods.SHA512));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA512")]
    public void TestHashSHA512_StandardStringHashModified_VerifyIsFalse() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.SHA512);
      string TestHash = Base64Hash + "0";
      Assert.IsFalse(SourceString.VerifyHashFromBase64(TestHash, THashingMethods.SHA512));
    }  
    #endregion SHA512

    #region RIPEMD160
    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("RIPEMD160")]
    public void TestHashRIPEMD160_StandardString_HashIsOK() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.RIPEMD160);
      Assert.IsTrue(SourceString.VerifyHashFromBase64(Base64Hash, THashingMethods.RIPEMD160));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("RIPEMD160")]
    public void TestHashRIPEMD160_StandardStringModifiedForVerify_VerifyIsFalse() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.RIPEMD160);
      string TestString = SourceString + "0";
      Assert.IsFalse(TestString.VerifyHashFromBase64(Base64Hash, THashingMethods.RIPEMD160));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("RIPEMD160")]
    public void TestHashRIPEMD160_StandardStringHashModified_VerifyIsFalse() {
      string SourceString = "This is a test";
      string Base64Hash = SourceString.HashToBase64(THashingMethods.RIPEMD160);
      string TestHash = Base64Hash + "0";
      Assert.IsFalse(SourceString.VerifyHashFromBase64(TestHash, THashingMethods.RIPEMD160));
    }
    #endregion RIPEMD160
  }
}
