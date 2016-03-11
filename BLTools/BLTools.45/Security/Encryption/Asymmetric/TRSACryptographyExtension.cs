using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;

namespace BLTools.Encryption {
  /// <summary>
  /// Manage RSA asymmetric encryption
  /// </summary>
  public static class TRSACryptographyExtension {

    static TRSACryptographyExtension()  {
    }


    #region Encrypt
    /// <summary>
    /// Encrypt a string to base64 using RSA and Encoding.Default
    /// </summary>
    /// <param name="source">The string to encrypt</param>
    /// <param name="targetPublicKeyXml">The public key to use, in XML string format</param>
    /// <returns>The encrypted base64 string</returns>
    public static string EncryptToRSABase64(this string source, TRSAPublicKey publicKey) {
      return TRSAEncryptor.EncryptToBase64(source, publicKey, Encoding.Default);
    }

    /// <summary>
    /// Encrypt a string to base64 using RSA
    /// </summary>
    /// <param name="source">The string to encrypt</param>
    /// <param name="targetPublicKeyXml">The public key to use, in XML string format</param>
    /// <param name="encoding">The encoding used in the source string</param>
    /// <returns>The encrypted base64 string</returns>
    public static string EncryptToRSABase64(this string source, TRSAPublicKey publicKey, Encoding encoding) {
      return TRSAEncryptor.EncryptToBase64(source, publicKey, encoding);
    }

    /// <summary>
    /// Encrypt a string to base64 using RSA and Encoding.Default
    /// </summary>
    /// <param name="source">The string to encrypt</param>
    /// <param name="publicKey">The public key to use, in RSAParameters format</param>
    /// <returns>The encrypted base64 string</returns>
    public static string EncryptToRSABase64(this string source, RSAParameters publicKey) {
      return TRSAEncryptor.EncryptToBase64(source, publicKey, Encoding.Default);
    }

    /// <summary>
    /// Encrypt a string to base64 using RSA
    /// </summary>
    /// <param name="source">The string to encrypt</param>
    /// <param name="publicKey">The public key to use, in RSAParameters format</param>
    /// <param name="encoding">The encoding used in the source string</param>
    /// <returns>The encrypted base64 string</returns>
    public static string EncryptToRSABase64(this string source, RSAParameters publicKey, Encoding encoding) {
      return TRSAEncryptor.EncryptToBase64(source, publicKey, encoding);
    }
    #endregion Encrypt

    #region Decrypt
    /// <summary>
    /// Decrypt a base64 RSA encrypted string using Encoding.Default
    /// </summary>
    /// <param name="base64Source">The base64 string to decrypt</param>
    /// <param name="privateKey">The private key to use</param>
    /// <returns>The decrypted string</returns>
    public static string DecryptFromRSABase64(this string base64Source, TRSAPrivateKey privateKey) {
      return TRSAEncryptor.DecryptFromBase64(base64Source, privateKey, Encoding.Default);
    }

    /// <summary>
    /// Decrypt a base64 RSA encrypted string
    /// </summary>
    /// <param name="base64Source">The base64 string to decrypt</param>
    /// <param name="privateKey">The private key to use</param>
    /// <param name="encoding">The encoding to use with the decrypted string</param>
    /// <returns>The decrypted string</returns>
    public static string DecryptFromRSABase64(this string base64Source, TRSAPrivateKey privateKey, Encoding encoding) {
      return TRSAEncryptor.DecryptFromBase64(base64Source, privateKey, encoding);
    }

    /// <summary>
    /// Decrypt a base64 RSA encrypted string using Encoding.Default
    /// </summary>
    /// <param name="base64Source">The base64 string to decrypt</param>
    /// <param name="privateKey">The private key to use, in RSAParameters format</param>
    /// <returns>The decrypted string</returns>
    public static string DecryptFromRSABase64(this string base64Source, RSAParameters privateKey) {
      return TRSAEncryptor.DecryptFromBase64(base64Source, privateKey, Encoding.Default);
    }

    /// <summary>
    /// Decrypt a base64 RSA encrypted string
    /// </summary>
    /// <param name="base64Source">The base64 string to decrypt</param>
    /// <param name="privateKey">The private key to use, in RSAParameters format</param>
    /// <param name="encoding">The encoding to use with the decrypted string</param>
    /// <returns>The decrypted string</returns>
    public static string DecryptFromRSABase64(this string base64Source, RSAParameters privateKey, Encoding encoding) {
      return TRSAEncryptor.DecryptFromBase64(base64Source, privateKey, encoding);
    }
    #endregion Decrypt

    #region Sign
    /// <summary>
    /// Generate a signature in base64 using RSA and Encoding.Default
    /// </summary>
    /// <param name="source">The text to sign</param>
    /// <param name="privateKey">The private key to use</param>
    /// <returns>A signature in base64</returns>
    public static string SignToRSABase64(this string source, TRSAPrivateKey privateKey) {
      return TRSAEncryptor.SignToBase64(source, privateKey, Encoding.Default);
    }

    /// <summary>
    /// Generate a signature in base64 using RSA
    /// </summary>
    /// <param name="source">The text to sign</param>
    /// <param name="privateKey">The private key to use</param>
    /// <param name="encoding">The encoding used in the text to sign</param>
    /// <returns>A signature in base64</returns>
    public static string SignToRSABase64(this string source, TRSAPrivateKey privateKey, Encoding encoding) {
      return TRSAEncryptor.SignToBase64(source, privateKey, encoding);
    }

    /// <summary>
    /// Generate a signature in base64 using RSA and Encoding.Default
    /// </summary>
    /// <param name="source">The text to sign</param>
    /// <param name="privateKey">The private key to use, in RSAParameters format</param>
    /// <returns>A signature in base64</returns>
    public static string SignToRSABase64(this string source, RSAParameters privateKey) {
      return TRSAEncryptor.SignToBase64(source, privateKey, Encoding.Default);
    }

    /// <summary>
    /// Generate a signature in base64 using RSA
    /// </summary>
    /// <param name="source">The text to sign</param>
    /// <param name="privateKey">The private key to use, in RSAParameters format</param>
    /// <param name="encoding">The encoding used in the text to sign</param>
    /// <returns>A signature in base64</returns>
    public static string SignToRSABase64(this string source, RSAParameters privateKey, Encoding encoding) {
      return TRSAEncryptor.SignToBase64(source, privateKey, encoding);
    }
    #endregion Sign

    #region Validate signature
    /// <summary>
    /// Validate a base64 signature using RSA and Encoding.Default
    /// </summary>
    /// <param name="source">The original text that was signed</param>
    /// <param name="signatureBase64ToValidate">The base64 signature to validate</param>
    /// <param name="publicKey">The public key to use</param>
    /// <returns>True if the signature is valid and original text has not been tampered. False otherwise.</returns>
    public static bool IsSignatureRSABase64Valid(this string source, string signatureBase64ToValidate, TRSAPublicKey publicKey) {
      return TRSAEncryptor.IsSignatureBase64Valid(source, signatureBase64ToValidate, publicKey, Encoding.Default);
    }

    /// <summary>
    /// Validate a base64 signature using RSA
    /// </summary>
    /// <param name="source">The original text that was signed</param>
    /// <param name="signatureBase64ToValidate">The base64 signature to validate</param>
    /// <param name="publicKey">The public key to use</param>
    /// <param name="encoding">The encoding used in the original text</param>
    /// <returns>True if the signature is valid and original text has not been tampered. False otherwise.</returns>
    public static bool IsSignatureRSABase64Valid(this string source, string signatureBase64ToValidate, TRSAPublicKey publicKey, Encoding encoding) {
      return TRSAEncryptor.IsSignatureBase64Valid(source, signatureBase64ToValidate, publicKey, encoding);
    }

    /// <summary>
    /// Validate a base64 signature using RSA and Encoding.Default
    /// </summary>
    /// <param name="source">The original text that was signed</param>
    /// <param name="signatureBase64ToValidate">The base64 signature to validate</param>
    /// <param name="publicKey">The public key to use, in RSAParameters format</param>
    /// <returns>True if the signature is valid and original text has not been tampered. False otherwise.</returns>
    public static bool IsSignatureRSABase64Valid(this string source, string signatureBase64ToValidate, RSAParameters publicKey) {
      return TRSAEncryptor.IsSignatureBase64Valid(source, signatureBase64ToValidate, publicKey, Encoding.Default);
    }

    /// <summary>
    /// Validate a base64 signature using RSA
    /// </summary>
    /// <param name="source">The original text that was signed</param>
    /// <param name="signatureBase64ToValidate">The base64 signature to validate</param>
    /// <param name="publicKey">The public key to use, in RSAParameters format</param>
    /// <param name="encoding">The encoding used in the original text</param>
    /// <returns>True if the signature is valid and original text has not been tampered. False otherwise.</returns>
    public static bool IsSignatureRSABase64Valid(this string source, string signatureBase64ToValidate, RSAParameters publicKey, Encoding encoding) {
      return TRSAEncryptor.IsSignatureBase64Valid(source, signatureBase64ToValidate, publicKey, encoding);
    }
    #endregion Validate signature

  }

}
