using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Encryption {
  public static class TSymmetricDecryption {

    public static string DecryptFromBase64(this string source, string password, TSymmetricEncryptionAlgorithm encryptionAlgorithm = TSymmetricEncryptionAlgorithm.AES, int keyLength = 256) {
      return source.DecryptFromBase64(password, Encoding.Default, encryptionAlgorithm, keyLength);
    }
    public static string DecryptFromBase64(this string source, string password, Encoding encoding, TSymmetricEncryptionAlgorithm encryptionAlgorithm = TSymmetricEncryptionAlgorithm.AES, int keyLength = 256) {
      #region Validate parameters
      if (source == null) {
        string Msg = string.Format("Unable to encrypt null data");
        Trace.WriteLine(Msg);
        throw new ArgumentNullException("source", Msg);
      }
      if (password == null) {
        string Msg = string.Format("Unable to encrypt data with a null password");
        Trace.WriteLine(Msg);
        throw new ArgumentNullException("password", Msg);
      }
      TSymmetricUtils.CheckKeyLength(keyLength, encryptionAlgorithm);
      #endregion Validate parameters

      string RetVal = "";
      TPIV PIVData;
      byte[] SourceBytes = Convert.FromBase64String(source);

      switch (encryptionAlgorithm) {
        case TSymmetricEncryptionAlgorithm.AES:
          PIVData = TPIV.Generate(password, keyLength / 8, 16);
          using (AesManaged AesDecoder = new AesManaged()) {
            Byte[] DecodedBytes = Decrypt(SourceBytes, AesDecoder, PIVData);
            if (DecodedBytes != null) {
              RetVal = encoding.GetString(DecodedBytes);
            } else {
              RetVal = null;
            }
          }
          return RetVal;

        case TSymmetricEncryptionAlgorithm.DES:
          PIVData = TPIV.Generate(password, 8, 8);
          using (DESCryptoServiceProvider DesDecoder = new DESCryptoServiceProvider()) {
            Byte[] DecodedBytes = Decrypt(SourceBytes, DesDecoder, PIVData);
            if (DecodedBytes != null) {
              RetVal = encoding.GetString(DecodedBytes);
            } else {
              RetVal = null;
            }
          }
          return RetVal;

        case TSymmetricEncryptionAlgorithm.TripleDES:
          PIVData = TPIV.Generate(password, keyLength / 8, 8);
          using (TripleDESCryptoServiceProvider TripleDesDecoder = new TripleDESCryptoServiceProvider()) {
            Byte[] DecodedBytes = Decrypt(SourceBytes, TripleDesDecoder, PIVData);
            if (DecodedBytes != null) {
              RetVal = encoding.GetString(DecodedBytes);
            } else {
              RetVal = null;
            }
          }
          return RetVal;

        case TSymmetricEncryptionAlgorithm.Rijndael:
          PIVData = TPIV.Generate(password, keyLength / 8, 16);
          using (RijndaelManaged RijndaelDecoder = new RijndaelManaged()) {
            Byte[] DecodedBytes = Decrypt(SourceBytes, RijndaelDecoder, PIVData);
            if (DecodedBytes != null) {
              RetVal = encoding.GetString(DecodedBytes);
            } else {
              RetVal = null;
            }
          }
          return RetVal;

        case TSymmetricEncryptionAlgorithm.RC2:
          PIVData = TPIV.Generate(password, keyLength / 8, 16);
          using (RC2CryptoServiceProvider RC2Decoder = new RC2CryptoServiceProvider()) {
            Byte[] DecodedBytes = Decrypt(SourceBytes, RC2Decoder, PIVData);
            if (DecodedBytes != null) {
              RetVal = encoding.GetString(DecodedBytes);
            } else {
              RetVal = null;
            }
          }
          return RetVal;
      }
      return "";
    }

    public static byte[] Decrypt(byte[] sourceBytes, SymmetricAlgorithm encoder, TPIV pivData) {
      byte[] RetVal;
      encoder.Mode = CipherMode.CBC;
      encoder.Padding = PaddingMode.PKCS7;
      try {
        using (MemoryStream EncodedStream = new MemoryStream(sourceBytes)) {
          using (CryptoStream DecryptorStream = new CryptoStream(EncodedStream, encoder.CreateDecryptor(pivData.Password, pivData.IV), CryptoStreamMode.Read)) {
            using (MemoryStream DecodedStream = new MemoryStream()) {
              DecryptorStream.CopyTo(DecodedStream);
              RetVal = DecodedStream.ToArray();
            }
          }
        }
        return RetVal;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to decipher from source bytes, check password or algorithm : {0}", ex.Message));
        return null;
      } finally {
        if (encoder != null) {
          encoder.Clear();
        }
      }
    }
  }
}
