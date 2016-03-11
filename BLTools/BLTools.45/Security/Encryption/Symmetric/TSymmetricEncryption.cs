using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Encryption {

  public static class TSymmetricEncryption {

    public static string EncryptToBase64(this string source, string password, TSymmetricEncryptionAlgorithm encryptionAlgorithm = TSymmetricEncryptionAlgorithm.AES, int keyLength = 256) {
      return source.EncryptToBase64(password, Encoding.Default, encryptionAlgorithm, keyLength);
    }
    public static string EncryptToBase64(this string source, string password, Encoding encoding, TSymmetricEncryptionAlgorithm encryptionAlgorithm = TSymmetricEncryptionAlgorithm.AES, int keyLength = 256) {
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
      byte[] SourceBytes = encoding.GetBytes(source);

      switch (encryptionAlgorithm) {

        case TSymmetricEncryptionAlgorithm.AES:
          PIVData = TPIV.Generate(password, keyLength / 8, 16);
          using (AesManaged AesEncoder = new AesManaged()) {
            RetVal = Convert.ToBase64String(EncryptToBytes(SourceBytes, AesEncoder, PIVData));
          }
          return RetVal;

        case TSymmetricEncryptionAlgorithm.DES:
          PIVData = TPIV.Generate(password, 8, 8);
          using (DESCryptoServiceProvider DesEncoder = new DESCryptoServiceProvider()) {
            RetVal = Convert.ToBase64String(EncryptToBytes(SourceBytes, DesEncoder, PIVData));
          }
          return RetVal;

        case TSymmetricEncryptionAlgorithm.TripleDES:
          PIVData = TPIV.Generate(password, keyLength / 8, 8);
          using (TripleDESCryptoServiceProvider TripleDesEncoder = new TripleDESCryptoServiceProvider()) {
            RetVal = Convert.ToBase64String(EncryptToBytes(SourceBytes, TripleDesEncoder, PIVData));
          }
          return RetVal;

        case TSymmetricEncryptionAlgorithm.Rijndael:
          PIVData = TPIV.Generate(password, keyLength / 8, 16);
          using (RijndaelManaged RijndaelEncoder = new RijndaelManaged()) {
            RetVal = Convert.ToBase64String(EncryptToBytes(SourceBytes, RijndaelEncoder, PIVData));
          }
          return RetVal;

        case TSymmetricEncryptionAlgorithm.RC2:
          PIVData = TPIV.Generate(password, keyLength / 8, 16);
          using (RC2CryptoServiceProvider RC2Encoder = new RC2CryptoServiceProvider()) {
            RetVal = Convert.ToBase64String(EncryptToBytes(SourceBytes, RC2Encoder, PIVData));
          }
          return RetVal;
      }
      return "";

    }

    public static byte[] EncryptToBytes(byte[] sourceBytes, SymmetricAlgorithm encoder, TPIV piv) {
      byte[] RetVal;
      encoder.Mode = CipherMode.CBC;
      encoder.Padding = PaddingMode.PKCS7;
      try {
        using (MemoryStream EncodedStream = new MemoryStream()) {
          using (CryptoStream EncryptorStream = new CryptoStream(EncodedStream, encoder.CreateEncryptor(piv.Password, piv.IV), CryptoStreamMode.Write)) {
            EncryptorStream.Write(sourceBytes, 0, sourceBytes.Length);
            EncryptorStream.FlushFinalBlock();
          }
          RetVal = EncodedStream.ToArray();
        }
        return RetVal;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to cipher to byte[], check password or algorithm : {0}", ex.Message));
        return null;
      } finally {
        if (encoder != null) {
          encoder.Clear();
        }
      }
    }



  }

}
