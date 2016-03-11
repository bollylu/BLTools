using BLTools.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Security.Cryptography;

namespace UnitTest2015 {


  /// <summary>
  ///This is a test class for TRSACryptographyExtensionTest and is intended
  ///to contain all TRSACryptographyExtensionTest Unit Tests
  ///</summary>
  [TestClass()]
  public class TRSACryptographyExtensionTest {


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

    [TestMethod(), TestCategory("RSA")]
    public void TRSAEncryptor_EncryptToBase64thenDecrypt_ResultIsIdentical() {
      TRSAKeyPair KeyPair = new TRSAKeyPair("testkey", TRSAKeyPair.KeyTypeEnum.Random);
      string SourceText = "this is a nice small text";
      string Target = SourceText.EncryptToRSABase64(KeyPair.PublicKey, Encoding.Default);
      Assert.AreEqual(SourceText, Target.DecryptFromRSABase64(KeyPair.PrivateKey));
    }

    [TestMethod(), TestCategory("RSA")]
    public void TRSAEncryptor_EncryptToBase64thenDecryptWithBadKey_ResultIsWrong() {
      TRSAKeyPair KeyPair1 = new TRSAKeyPair("testkey1", TRSAKeyPair.KeyTypeEnum.Random);
      TRSAKeyPair KeyPair2 = new TRSAKeyPair("testkey2", TRSAKeyPair.KeyTypeEnum.Random);
      string SourceText = "this is a nice small text";
      string Target = SourceText.EncryptToRSABase64(KeyPair1.PublicKey, Encoding.Default);
      Assert.AreNotEqual(SourceText, Target.DecryptFromRSABase64(KeyPair2.PrivateKey));
    }

    [TestMethod(), TestCategory("RSA")]
    public void TRSAEncryptor_SignThenCheckSignature_SignatureIsOK() {
      TRSAKeyPair KeyPair = new TRSAKeyPair("testkey", TRSAKeyPair.KeyTypeEnum.Random);
      string SourceText = "this is a nice small text";
      string Signature = SourceText.SignToRSABase64(KeyPair.PrivateKey, Encoding.Default);
      Assert.IsTrue(SourceText.IsSignatureRSABase64Valid(Signature, KeyPair.PublicKey, Encoding.Default));
    }

    [TestMethod(), TestCategory("RSA")]
    public void TRSAEncryptor_SignThenModifyThenCheckSignature_SignatureIsBad() {
      TRSAKeyPair KeyPair = new TRSAKeyPair("testkey", TRSAKeyPair.KeyTypeEnum.Random);
      string SourceText = "this is a nice small text";
      string ModifiedText = SourceText.ToUpper();
      string Signature = SourceText.SignToRSABase64(KeyPair.PrivateKey, Encoding.Default);
      Assert.IsFalse(ModifiedText.IsSignatureRSABase64Valid(Signature, KeyPair.PublicKey, Encoding.Default));
    }



  }
}
