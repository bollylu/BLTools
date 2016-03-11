using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLTools.Encryption;
using System.Diagnostics;
using System.Text;

namespace UnitTest2015 {
  [TestClass]
  public class TSymmetricEncryption3DesTest {
    [TestMethod(), TestCategory("SymmetricEncryption"), TestCategory("3DES")]
    public void TestEncryptSymmetric3Des_ParametersOk128_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.TripleDES, 128);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, TSymmetricEncryptionAlgorithm.TripleDES, 128);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("3DES")]
    public void TestEncryptSymmetric3Des_ParametersOk192_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("3DES")]
    public void TestEncryptSymmetric3Des_SourceEmpty_EncryptionDecryptionOK() {
      string SourceText = "";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      Assert.AreEqual(SourceText, DecipheredText);
    }
    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("3DES")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetric3Des_BadKeyLengthTooSmall_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.TripleDES, 125);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("3DES")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetric3Des_BadKeyLengthZero_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.TripleDES, 0);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("3DES")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetric3Des_BadKeyLengthTooBig_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.TripleDES, 1024);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("3DES")]
    public void TestEncryptSymmetric3Des_NoPassword_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("3DES")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestEncryptSymmetric3Des_SourceTextIsNull_Exception() {
      string SourceText = null;
      string Password = "";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.TripleDES, 192);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("3DES")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestEncryptSymmetric3Des_NullPassword_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = null;
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.TripleDES, 192);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("3DES")]
    public void TestEncryptSymmetric3Des_WrongPassword_DecryptionFailed() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      string DecryptPassword = "az12df34vc";
      string DecipheredText = EncryptedBase64.DecryptFromBase64(DecryptPassword, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      Assert.IsNull(DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("3DES")]
    public void TestEncryptSymmetric3Des_ParametersOKEncodingUTF8_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.UTF8, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.UTF8, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("3DES")]
    public void TestEncryptSymmetric3Des_ParametersOKWrongEncoding_DecryptionFailed() {
      string SourceText = "Je vais bien, merci. Célébration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.Default, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      Assert.AreNotEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("3DES")]
    public void TestEncryptSymmetric3Des_ParametersASCIIEncodingASCII_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci. Celebration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("3DES")]
    public void TestEncryptSymmetric3Des_ParametersASCII_Accents_EncodingASCII_DecryptionFailed() {
      string SourceText = "Je vais bien, merci. Célébration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.ASCII, TSymmetricEncryptionAlgorithm.TripleDES, 192);
      Assert.AreNotEqual(SourceText, DecipheredText);
    }
  }
}
