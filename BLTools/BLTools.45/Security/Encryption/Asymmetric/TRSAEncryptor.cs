using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;

namespace BLTools.Encryption {
  internal class TRSAEncryptor {

    #region Encrypt
    internal static string EncryptToBase64(string source, TRSAPublicKey publicKey, Encoding encoding) {
      string RetVal = null;
      using (RSACryptoServiceProvider TempRSACSP = new RSACryptoServiceProvider()) {
        TempRSACSP.FromXmlString(publicKey.Key);
        RetVal = EncryptToBase64(source, TempRSACSP.ExportParameters(false), encoding);
      }
      return RetVal;
    }
    internal static string EncryptToBase64(string source, RSAParameters publicKey, Encoding encoding) {
      #region Validate parameters
      if (source == null) {
        Trace.WriteLine("Error: Unable to encrypt a null string");
        throw new ArgumentException("Unable to encrypt a null string", "source");
      }
      #endregion Validate parameters

      string RetVal = null;
      try {
        using (RSACryptoServiceProvider RSACSP = new RSACryptoServiceProvider()) {
          RSACSP.ImportParameters(publicKey);
          byte[] EncryptedData = RSACSP.Encrypt(encoding.GetBytes(source), true);
          RetVal = Convert.ToBase64String(EncryptedData);
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error: unable to encrypt data : {0}", ex.Message));
        RetVal = null;
      }

      return RetVal;
    }
    #endregion Encrypt

    #region Decrypt
    internal static string DecryptFromBase64(string base64Source, TRSAPrivateKey privateKey, Encoding encoding) {
      string RetVal = null;
      using (RSACryptoServiceProvider TempRSACSP = new RSACryptoServiceProvider()) {
        TempRSACSP.FromXmlString(privateKey.Key);
        RetVal = DecryptFromBase64(base64Source, TempRSACSP.ExportParameters(true), encoding);
      }
      return RetVal;
    }
    internal static string DecryptFromBase64(string base64Source, RSAParameters targetKey, Encoding encoding) {
      #region Validate parameters
      if (base64Source == null) {
        Trace.WriteLine("Error: Unable to decrypt a null string");
        throw new ArgumentException("Unable to encrypt a null string", "base64Source");
      }
      #endregion Validate parameters

      string RetVal = null;
      try {
        using (RSACryptoServiceProvider RSACSP = new RSACryptoServiceProvider()) {
          RSACSP.ImportParameters(targetKey);
          byte[] DecryptedData = RSACSP.Decrypt(Convert.FromBase64String(base64Source), true);
          RetVal = encoding.GetString(DecryptedData);
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error: unable to encrypt data : {0}", ex.Message));
        RetVal = null;
      }

      return RetVal;
    }
    #endregion Decrypt

    #region Sign
    internal static string SignToBase64(string source, TRSAPrivateKey privateKey, Encoding encoding) {
      #region Validate parameters
      if (source == null) {
        string Msg = "Error: Unable to sign a null string";
        Trace.WriteLine(Msg);
        throw new ArgumentException(Msg, "source");
      }

      if (privateKey == null) {
        string Msg = "Error: Unable to create a signature with a null private key";
        Trace.WriteLine(Msg);
        throw new ArgumentException(Msg, "privateKey");
      }
      #endregion Validate parameters

      string RetVal = null;
      using (RSACryptoServiceProvider TempRSACSP = new RSACryptoServiceProvider()) {
        TempRSACSP.FromXmlString(privateKey.Key);
        RetVal = SignToBase64(source, TempRSACSP.ExportParameters(true), encoding);
      }
      return RetVal;
    }
    internal static string SignToBase64(string source, RSAParameters privateKey, Encoding encoding) {
      #region Validate parameters
      if (source == null) {
        Trace.WriteLine("Error: Unable to sign a null string");
        throw new ArgumentException("Unable to sign a null string", "source");
      }
      #endregion Validate parameters

      string RetVal = null;

      try {
        using (RSACryptoServiceProvider RSACSP = new RSACryptoServiceProvider()) {
          RSACSP.ImportParameters(privateKey);
          byte[] Signature = RSACSP.SignData(encoding.GetBytes(source), new SHA1CryptoServiceProvider());
          RetVal = Convert.ToBase64String(Signature);
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error: unable to sign data : {0}", ex.Message));
        RetVal = null;
      }

      return RetVal;
    }
    #endregion Sign

    #region Validate signature
    internal static bool IsSignatureBase64Valid(string source, string base64Signature, TRSAPublicKey publicKey, Encoding encoding) {
      
      #region Validate parameters
      if (source == null) {
        string Msg = "Error: Unable to validate the signature of a null string";
        Trace.WriteLine(Msg);
        throw new ArgumentException(Msg, "source");
      }

      if (string.IsNullOrEmpty(base64Signature)) {
        string Msg = "Unable to validate an empty or null signature";
        Trace.WriteLine(Msg);
        throw new ArgumentException(Msg, "base64Signature");
      }

      if (publicKey == null) {
        string Msg = "Error: Unable to validate a signature with a null public key";
        Trace.WriteLine(Msg);
        throw new ArgumentException(Msg, "publicKey");
      } 
      #endregion Validate parameters

      bool RetVal = false;

      using (RSACryptoServiceProvider TempRSACSP = new RSACryptoServiceProvider()) {
        TempRSACSP.FromXmlString(publicKey.Key);
        RetVal = IsSignatureBase64Valid(source, base64Signature, TempRSACSP.ExportParameters(false), encoding);
      }
      return RetVal;
    }

    internal static bool IsSignatureBase64Valid(string source, string base64Signature, RSAParameters publicKey, Encoding encoding) {
      
      #region Validate parameters
      if (source == null) {
        string Msg = "Error: Unable to validate the signature of a null string";
        Trace.WriteLine(Msg);
        throw new ArgumentException(Msg, "source");
      }

      if (string.IsNullOrEmpty(base64Signature)) {
        string Msg = "Unable to validate an empty or null signature";
        Trace.WriteLine(Msg);
        throw new ArgumentException(Msg, "base64Signature");
      } 
      #endregion Validate parameters

      bool RetVal = false;

      try {
        using (RSACryptoServiceProvider RSACSP = new RSACryptoServiceProvider()) {
          RSACSP.ImportParameters(publicKey);
          RetVal = RSACSP.VerifyData(encoding.GetBytes(source), new SHA1CryptoServiceProvider(), Convert.FromBase64String(base64Signature));
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error: unable to validate signature : {0}", ex.Message));
        RetVal = false;
      }

      return RetVal;
    }
    #endregion Validate signature
  }
}
