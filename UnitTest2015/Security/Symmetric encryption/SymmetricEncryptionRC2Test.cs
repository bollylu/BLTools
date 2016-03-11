using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLTools.Encryption;
using System.Diagnostics;
using System.Text;

namespace UnitTest2015 {
  [TestClass]
  public class TSymmetricEncryptionRC2Test {
    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    public void TestEncryptSymmetricRC2_ParametersOk40_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 40);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 40);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    public void TestEncryptSymmetricRC2_ParametersOk64_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 64);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 64);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    public void TestEncryptSymmetricRC2_ParametersOk128_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 128);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 128);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    public void TestEncryptSymmetricRC2_SourceEmpty_EncryptionDecryptionOK() {
      string SourceText = "";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 128);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 128);
      Assert.AreEqual(SourceText, DecipheredText);
    }
    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetricRC2_BadKeyLengthTooSmall_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 127);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetricRC2_BadKeyLengthZero_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 0);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetricRC2_BadKeyLengthTooBig_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 1024);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    public void TestEncryptSymmetricRC2_NoPassword_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 128);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 128);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestEncryptSymmetricRC2_SourceTextIsNull_Exception() {
      string SourceText = null;
      string Password = "";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 128);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestEncryptSymmetricRC2_NullPassword_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = null;
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 128);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    public void TestEncryptSymmetricRC2_WrongPassword_DecryptionFailed() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.RC2, 128);
      string DecryptPassword = "az12df34vc";
      string DecipheredText = EncryptedBase64.DecryptFromBase64(DecryptPassword, TSymmetricEncryptionAlgorithm.RC2, 128);
      Assert.IsNull(DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    public void TestEncryptSymmetricRC2_ParametersOKEncodingUTF8_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.UTF8, TSymmetricEncryptionAlgorithm.RC2, 128);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.UTF8, TSymmetricEncryptionAlgorithm.RC2, 128);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    public void TestEncryptSymmetricRC2_ParametersOKWrongEncoding_DecryptionFailed() {
      string SourceText = "Je vais bien, merci. Célébration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.RC2, 128);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.Default, TSymmetricEncryptionAlgorithm.RC2, 128);
      Assert.AreNotEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    public void TestEncryptSymmetricRC2_ParametersASCIIEncodingASCII_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci. Celebration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.RC2, 128);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.RC2, 128);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("RC2")]
    public void TestEncryptSymmetricRC2_ParametersASCII_Accents_EncodingASCII_DecryptionFailed() {
      string SourceText = "Je vais bien, merci. Célébration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.RC2, 128);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.RC2, 128);
      Assert.AreNotEqual(SourceText, DecipheredText);
    }
  }
}
