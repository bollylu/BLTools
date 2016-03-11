using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLTools.Encryption;
using System.Diagnostics;
using System.Text;

namespace UnitTest2015 {
  [TestClass]
  public class TSymmetricEncryptionRijndaelTest {
    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    public void TestEncryptSymmetricRijndael_ParametersOk128_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 128);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 128);
      Assert.AreEqual(SourceText, DecipheredText);
    }
    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    public void TestEncryptSymmetricRijndael_ParametersOk192_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 192);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 192);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    public void TestEncryptSymmetricRijndael_ParametersOk256_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    public void TestEncryptSymmetricRijndael_SourceEmpty_EncryptionDecryptionOK() {
      string SourceText = "";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      Assert.AreEqual(SourceText, DecipheredText);
    }
    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetricRijndael_BadKeyLengthTooSmall_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 253);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetricRijndael_BadKeyLengthZero_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 0);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetricRijndael_BadKeyLengthTooBig_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 1024);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    public void TestEncryptSymmetricRijndael_NoPassword_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestEncryptSymmetricRijndael_SourceTextIsNull_Exception() {
      string SourceText = null;
      string Password = "";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 256);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestEncryptSymmetricRijndael_NullPassword_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = null;
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 256);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    public void TestEncryptSymmetricRijndael_WrongPassword_DecryptionFailed() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      string DecryptPassword = "az12df34vc";
      string DecipheredText = EncryptedBase64.DecryptFromBase64(DecryptPassword, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      Assert.IsNull(DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    public void TestEncryptSymmetricRijndael_ParametersOKEncodingUTF8_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.UTF8, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.UTF8, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    public void TestEncryptSymmetricRijndael_ParametersOKWrongEncoding_DecryptionFailed() {
      string SourceText = "Je vais bien, merci. Célébration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.Default, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      Assert.AreNotEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    public void TestEncryptSymmetricRijndael_ParametersASCIIEncodingASCII_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci. Celebration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("Rijndael")]
    public void TestEncryptSymmetricRijndael_ParametersASCII_Accents_EncodingASCII_DecryptionFailed() {
      string SourceText = "Je vais bien, merci. Célébration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.Rijndael, 256);
      Assert.AreNotEqual(SourceText, DecipheredText);
    }
  }
}
