using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLTools.Encryption;

namespace UnitTest2015 {
  [TestClass]
  public class THashMacExtensionStringTest {
    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("MD5")]
    public void TestHMACMD5_StandardString_HMacIsOK() {
      string SourceString = "The quick brown fox jumps over the lazy dog";
      string Key = "key";
      string Base64HMac = SourceString.HMacToBase64(Key, THashingMethods.MD5);
      Assert.IsTrue(SourceString.VerifyHMACFromBase64(Key, Base64HMac, THashingMethods.MD5));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA1")]
    public void TestHMACSHA1_StandardString_HMacIsOK() {
      string SourceString = "The quick brown fox jumps over the lazy dog";
      string Key = "key";
      string Base64HMac = SourceString.HMacToBase64(Key, THashingMethods.SHA1);
      Assert.IsTrue(SourceString.VerifyHMACFromBase64(Key, Base64HMac, THashingMethods.SHA1));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA256")]
    public void TestHMACSHA256_StandardString_HMacIsOK() {
      string SourceString = "The quick brown fox jumps over the lazy dog";
      string Key = "key";
      string Base64HMac = SourceString.HMacToBase64(Key, THashingMethods.SHA256);
      Assert.IsTrue(SourceString.VerifyHMACFromBase64(Key, Base64HMac, THashingMethods.SHA256));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA384")]
    public void TestHMACSHA384_StandardString_HMacIsOK() {
      string SourceString = "The quick brown fox jumps over the lazy dog";
      string Key = "key";
      string Base64HMac = SourceString.HMacToBase64(Key, THashingMethods.SHA384);
      Assert.IsTrue(SourceString.VerifyHMACFromBase64(Key, Base64HMac, THashingMethods.SHA384));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA512")]
    public void TestHMACSHA512_StandardString_HMacIsOK() {
      string SourceString = "The quick brown fox jumps over the lazy dog";
      string Key = "key";
      string Base64HMac = SourceString.HMacToBase64(Key, THashingMethods.SHA512);
      Assert.IsTrue(SourceString.VerifyHMACFromBase64(Key, Base64HMac, THashingMethods.SHA512));
    }

    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("RIPEMD160")]
    public void TestHMACRipeMD160_StandardString_HMacIsOK() {
      string SourceString = "The quick brown fox jumps over the lazy dog";
      string Key = "key";
      string Base64HMac = SourceString.HMacToBase64(Key, THashingMethods.RIPEMD160);
      Assert.IsTrue(SourceString.VerifyHMACFromBase64(Key, Base64HMac, THashingMethods.RIPEMD160));
    }


    [TestMethod(), TestCategory("Security"), TestCategory("Hash"), TestCategory("SHA256")]
    public void TestHMACSHA256_StandardStringWrongKeyForDecode_VerifyIsFalse() {
      string SourceString = "The quick brown fox jumps over the lazy dog";
      string Key = "key";
      string WrongKey = "anotherKey";
      string Base64HMac = SourceString.HMacToBase64(Key, THashingMethods.SHA256);
      Assert.IsFalse(SourceString.VerifyHMACFromBase64(WrongKey, Base64HMac, THashingMethods.SHA256));
    }
  }
}
