using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;

namespace BLTools.Encryption {
  //internal class TSymmetricEncryptor : IDisposable {

  //  /// <summary>
  //  /// Encrypts a string using 3DES and returns a base64 string
  //  /// </summary>
  //  /// <param name="source">The string to encrypt</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <param name="encoding">The Encoding</param>
  //  /// <returns>A base64 encrypted string</returns>
  //  internal string EncryptTo3DESBase64(string source, TSymmetricSecurityKey key, Encoding encoding) {
  //    #region Validate parameters
  //    if (source == null) {
  //      throw new ArgumentNullException("Error while encrypting string : source string is null", "source");
  //    }
  //    if (key == null) {
  //      throw new ArgumentNullException("Error while encrypting string : key is null", "key");
  //    }
      
  //    #endregion Validate parameters
  //    string RetVal;
  //    using (MemoryStream oMemoryStream = new MemoryStream()) {
  //      ICryptoTransform Alg3DES = null;
  //      try {
  //        Alg3DES = new TripleDESCryptoServiceProvider().CreateEncryptor(key.SecretKey, key.InitializationVector);
  //        using (CryptoStream oCryptoStream = new CryptoStream(oMemoryStream, Alg3DES, CryptoStreamMode.Write)) {
  //          byte[] DataToEncrypt = encoding.GetBytes(source);
  //          oCryptoStream.Write(DataToEncrypt, 0, DataToEncrypt.Length);
  //          oCryptoStream.FlushFinalBlock();
  //          RetVal = Convert.ToBase64String(oMemoryStream.ToArray());
  //        }
  //      } catch (CryptographicException ex) {
  //        Trace.WriteLine(string.Format("Cryptographic exception while encrypting to 3DES : {0}", ex.Message));
  //        Trace.WriteLine(string.Format("  Available keys length are {0}", string.Join(",", TripleDESCryptoServiceProvider.Create().LegalKeySizes.Select(x => string.Format("Min={0}:Max={1}:Skip={2}", x.MinSize, x.MaxSize, x.SkipSize)).ToArray())));
  //        RetVal = "";
  //      } catch (Exception ex) {
  //        Trace.WriteLine(string.Format("Generic exception while encrypting to 3DES : {0}", ex.Message));
  //        RetVal = "";
  //      }
  //    }
  //    return RetVal;
  //  }

  //  /// <summary>
  //  /// Decrypt a base64 string using 3DES
  //  /// </summary>
  //  /// <param name="source">The source string in base64</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <param name="encoding">The Encoding</param>
  //  /// <returns>A decrypted string</returns>
  //  internal string DecryptFrom3DESBase64(string source, TSymmetricSecurityKey key, Encoding encoding) {
  //    #region Validate parameters
  //    if (source == null) {
  //      throw new ArgumentNullException("Error while decrypting string : source string is null", "source");
  //    }
  //    if (key == null) {
  //      throw new ArgumentNullException("Error while decrypting string : key is null", "key");
  //    }

  //    #endregion Validate parameters
  //    string RetVal = "";
  //    try {
  //      using (MemoryStream m = new MemoryStream(Convert.FromBase64String(source))) {
  //        using (CryptoStream c = new CryptoStream(m, new TripleDESCryptoServiceProvider().CreateDecryptor(key.SecretKey, key.InitializationVector), CryptoStreamMode.Read)) {
  //          using (StreamReader w = new StreamReader(c, encoding)) {
  //            RetVal = w.ReadToEnd();
  //          }
  //        }
  //      }
  //    } catch (Exception ex) {
  //      Trace.WriteLine(string.Format("Error decrypting data : {0}", ex.Message));
  //      RetVal = "";
  //    }
  //    return RetVal;
  //  }

  //  /// <summary>
  //  /// Encrypt a string and store it into a file. The file is created and must not exist before.
  //  /// </summary>
  //  /// <param name="source">The string to encrypt</param>
  //  /// <param name="fileName">The filename to create</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <param name="encoding">The Encoding</param>
  //  internal void EncryptStringToFile(string source, string fileName, TSymmetricSecurityKey key, Encoding encoding) {
  //    #region Validate parameters
  //    if (source == null) {
  //      throw new ArgumentNullException("Error while encrypting string to a file: source string is null", "source");
  //    }
  //    if (fileName == null) {
  //      throw new ArgumentNullException("Error while encrypting string to a file: filename is null", "filename");
  //    }
  //    if (fileName == "") {
  //      throw new ArgumentException("Error while encrypting string to a file: filename must not be empty", "filename");
  //    }
  //    if (key == null) {
  //      throw new ArgumentNullException("Error while encrypting string from a file : key is null", "key");
  //    }
  //    if (encoding == null) {
  //      throw new ArgumentNullException("Error while encrypting string from a file : encoding is null", "encoding");
  //    }
  //    #endregion Validate parameters
  //    try {
  //      using (FileStream oFS = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None)) {
  //        try {
  //          using (CryptoStream oCryptoStream = new CryptoStream(oFS, new TripleDESCryptoServiceProvider().CreateEncryptor(key.SecretKey, key.InitializationVector), CryptoStreamMode.Write)) {
  //            oCryptoStream.Write(encoding.GetBytes(source), 0, encoding.GetBytes(source).Length);
  //            oCryptoStream.FlushFinalBlock();
  //          }
  //        } catch (Exception ex) {
  //          Trace.WriteLine(string.Format("Unable to write encrypted data in {0} : {1}", fileName, ex.Message));
  //          return;
  //        }
  //      }
  //    } catch (Exception ex) {
  //      Trace.WriteLine(string.Format("Unable to open {0} for writing encrypted data : {1}", fileName, ex.Message));
  //      return;
  //    }
  //    return;
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a string, provided SecurityKeys and Encoding
  //  /// Warning: Encrypted content should be a textual value, not a binary
  //  /// </summary>
  //  /// <param name="fileName">The filename to decrypt</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <param name="encoding">The Encoding</param>
  //  /// <returns>A string containg the decrypted content of the file</returns>
  //  internal string DecryptFileToString(string fileName, TSymmetricSecurityKey key, Encoding encoding) {
  //    #region >> Validate parameters <<
  //    if (fileName == null) {
  //      throw new ArgumentNullException("Error while decrypting string from a file: filename is null", "filename");
  //    }
  //    if (fileName == "") {
  //      throw new ArgumentException("Error while decrypting string from a file: filename must not be empty", "filename");
  //    }
  //    if (key == null) {
  //      throw new ArgumentNullException("Error while decrypting string from a file : key is null", "key");
  //    }
  //    if (encoding == null) {
  //      throw new ArgumentNullException("Error while decrypting string from a file : encoding is null", "encoding");
  //    }
  //    #endregion >> Validate parameters <<
  //    string RetVal = "";
  //    try {
  //      using (FileStream oFS = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
  //        try {
  //          using (CryptoStream oCryptoStream = new CryptoStream(oFS, new TripleDESCryptoServiceProvider().CreateDecryptor(key.SecretKey, key.InitializationVector), CryptoStreamMode.Read)) {
  //            byte[] ReadBuffer = new byte[oFS.Length];
  //            int ReadByteCount = oCryptoStream.Read(ReadBuffer, 0, (int)oFS.Length);
  //            RetVal = encoding.GetString(ReadBuffer.Take(ReadByteCount).ToArray());
  //          }
  //        } catch (Exception ex) {
  //          Trace.WriteLine(string.Format("Exception error during decryption of {0} : {1}", fileName, ex.Message));
  //          return null;
  //        }
  //      }
  //    } catch (Exception ex) {
  //      Trace.WriteLine(string.Format("Exception during opening of {0} for decryption : {1}", fileName, ex.Message));
  //      return null;
  //    }

  //    return RetVal;
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a string, provided SecurityKeys and Encoding
  //  /// Warning: Encrypted content should be a textual value, not a binary
  //  /// </summary>
  //  /// <param name="fileInfo">The filename to decrypt</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <param name="encoding">The Encoding</param>
  //  /// <returns>A string containg the decrypted content of the file</returns>
  //  internal string DecryptFileToString(FileInfo fileInfo, TSymmetricSecurityKey key, Encoding encoding) {
  //    return DecryptFileToString(fileInfo.FullName, key, encoding);
  //  }

  //  /// <summary>
  //  /// Encrypts a byte[] using 3DES and returns a base64 string
  //  /// </summary>
  //  /// <param name="source">The byte[] to encrypt</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <returns>A base64 encrypted string</returns>
  //  internal string EncryptTo3DESBase64(byte[] source, TSymmetricSecurityKey key) {
  //    #region Validate parameters
  //    if (source == null) {
  //      throw new ArgumentNullException("Error while encrypting : source byte[] is null", "source");
  //    }
  //    if (key == null) {
  //      throw new ArgumentNullException("Error while encrypting : SecurityKey is null", "key");
  //    }
  //    #endregion Validate parameters
  //    string RetVal;
  //    using (MemoryStream oMemoryStream = new MemoryStream()) {
  //      ICryptoTransform Alg3DES;
  //      try {
  //        Alg3DES = new TripleDESCryptoServiceProvider().CreateEncryptor(key.SecretKey, key.InitializationVector);
  //        using (CryptoStream oCryptoStream = new CryptoStream(oMemoryStream, Alg3DES, CryptoStreamMode.Write)) {
  //          //byte[] DataToEncrypt = encoding.GetBytes(source);
  //          oCryptoStream.Write(source, 0, source.Length);
  //          oCryptoStream.FlushFinalBlock();
  //          RetVal = Convert.ToBase64String(oMemoryStream.ToArray());
  //        }
  //      } catch (CryptographicException ex) {
  //        Trace.WriteLine(string.Format("Cryptographic exception while encrypting to 3DES : {0}", ex.Message));
  //        Trace.WriteLine(string.Format("  Available keys length are {0}", string.Join(",", TripleDESCryptoServiceProvider.Create().LegalKeySizes.Select(x => string.Format("Min={0}:Max={1}:Skip={2}", x.MinSize, x.MaxSize, x.SkipSize)).ToArray())));
  //        RetVal = "";
  //      } catch (Exception ex) {
  //        Trace.WriteLine(string.Format("Generic exception while encrypting to 3DES : {0}", ex.Message));
  //        RetVal = "";
  //      }
  //    }
  //    return RetVal;
  //  }

  //  /// <summary>
  //  /// Decrypt a base64 string using 3DES
  //  /// </summary>
  //  /// <param name="base64Source">The source string in base64</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <returns>A decrypted string</returns>
  //  internal byte[] DecryptFrom3DESBase64(string base64Source, TSymmetricSecurityKey key) {
  //    #region >> Validate parameters <<
  //    if (base64Source == null) {
  //      throw new ArgumentException("The source cannot be null", "base64Source");
  //    }
  //    if (key == null) {
  //      throw new ArgumentNullException("Error while decrypting : SecurityKey is null", "key");
  //    }
  //    if (base64Source == "") {
  //      return new byte[0];
  //    }
  //    #endregion >> Validate parameters <<
  //    byte[] RetVal = null;
  //    try {
  //      using (MemoryStream m = new MemoryStream(Convert.FromBase64String(base64Source))) {
  //        using (CryptoStream c = new CryptoStream(m, new TripleDESCryptoServiceProvider().CreateDecryptor(key.SecretKey, key.InitializationVector), CryptoStreamMode.Read)) {
  //          using (MemoryStream o = new MemoryStream()) {
  //            int DecryptedByte = c.ReadByte();
  //            while (DecryptedByte != -1) {
  //              o.WriteByte((byte)DecryptedByte);
  //              DecryptedByte = c.ReadByte();
  //            }
  //            RetVal = o.ToArray();
  //          }
  //        }
  //      }
  //    } catch (Exception ex) {
  //      Trace.WriteLine(string.Format("Error decrypting data : {0}", ex.Message));
  //      RetVal = null;
  //    }
  //    return RetVal;
  //  }

  //  /// <summary>
  //  /// Encrypt an array of bytes into a file using 3DES
  //  /// </summary>
  //  /// <param name="source">The source byte[]</param>
  //  /// <param name="fileName">The filename to create</param>
  //  /// <param name="key">The SecurityKey</param>
  //  /// <returns>True when successfull, false otherwise</returns>
  //  internal bool EncryptBytesToFile(byte[] source, string fileName, TSymmetricSecurityKey key) {
  //    #region >> Validate parameters <<
  //    if (source == null) {
  //      throw new ArgumentException("Error : Cannot encrypt a null byte[]", "source");
  //    }
  //    if (fileName == null) {
  //      throw new ArgumentNullException("Error while encrypting byte[] to a file: filename is null", "filename");
  //    }
  //    if (fileName == "") {
  //      throw new ArgumentException("Error while encrypting byte[] to a file: filename must not be empty", "filename");
  //    }
  //    if (key == null) {
  //      throw new ArgumentNullException("Error while encrypting : SecurityKey is null", "key");
  //    }
  //    #endregion >> Validate parameters <<
  //    try {
  //      using (FileStream oFS = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None)) {
  //        try {
  //          using (CryptoStream oCryptoStream = new CryptoStream(oFS, new TripleDESCryptoServiceProvider().CreateEncryptor(key.SecretKey, key.InitializationVector), CryptoStreamMode.Write)) {
  //            oCryptoStream.Write(source, 0, source.Length);
  //            oCryptoStream.FlushFinalBlock();
  //          }
  //        } catch (Exception ex) {
  //          Trace.WriteLine(string.Format("Unable to write encrypted data in {0} : {1}", fileName, ex.Message));
  //          return false;
  //        }
  //      }
  //    } catch (Exception ex) {
  //      Trace.WriteLine(string.Format("Unable to open {0} for writing encrypted data : {1}", fileName, ex.Message));
  //      return false;
  //    }
  //    return true;
  //  }

  //  /// <summary>
  //  /// 
  //  /// Decrypt an array of bytes from a file using 3DES
  //  /// </summary>
  //  /// <param name="fileName">The filename to create</param>
  //  /// <param name="key">The SecurityKey</param>
  //  /// <returns>The decrypted array of bytes</returns>
  //  internal byte[] DecryptFileToBytes(string fileName, TSymmetricSecurityKey key) {
  //    #region >> Validate parameters <<
  //    if (fileName == null) {
  //      throw new ArgumentNullException("Error while encrypting byte[] to a file: filename is null", "filename");
  //    }
  //    if (fileName == "") {
  //      throw new ArgumentException("Error while encrypting byte[] to a file: filename must not be empty", "filename");
  //    }
  //    if (key == null) {
  //      throw new ArgumentNullException("Error while encrypting : SecurityKey is null", "key");
  //    }
  //    if (!File.Exists(fileName)) {
  //      throw new ArgumentException(string.Format("Error : the specified file is missing  or access is denied : \"{0}\"", fileName));
  //    }
  //    #endregion >> Validate parameters <<
  //    byte[] RetVal = null;
  //    try {
  //      using (FileStream oFS = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
  //        try {
  //          using (CryptoStream oCryptoStream = new CryptoStream(oFS, new TripleDESCryptoServiceProvider().CreateDecryptor(key.SecretKey, key.InitializationVector), CryptoStreamMode.Read)) {
  //            byte[] ReadBuffer = new byte[oFS.Length];
  //            int ReadByteCount = oCryptoStream.Read(ReadBuffer, 0, (int)oFS.Length);
  //            RetVal = ReadBuffer.Take(ReadByteCount).ToArray();
  //          }
  //        } catch (Exception ex) {
  //          Trace.WriteLine(string.Format("Exception error during decryption of \"{0}\" : {1}", fileName, ex.Message));
  //          return null;
  //        }
  //      }
  //    } catch (Exception ex) {
  //      Trace.WriteLine(string.Format("Exception during opening of \"{0}\" for decryption : {1}", fileName, ex.Message));
  //      return null;
  //    }

  //    return RetVal;
  //  }

  //  public void Dispose() {
  //  }

  //}
}
