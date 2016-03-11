using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace BLTools.Encryption {
  /// <summary>
  /// Manage symmetric encryption and decryption
  /// </summary>
  //public static class TSymmetricSecurityExtension {

  //  #region >> Extension methods for strings <<

  //  #region >> Encryption <<
  //  /// <summary>
  //  /// Encrypts a string in 3DES mode and KeyType.Default and Encoding.Default
  //  /// </summary>
  //  /// <param name="source">Source string to encrypt</param>
  //  /// <returns>Encrypted string in base64 mode</returns>
  //  public static string EncryptTo3DESBase64(this string source) {
  //    return EncryptTo3DESBase64(source, new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Default));
  //  }

  //  /// <summary>
  //  /// Encrypts a string in 3DES mode and Encoding.Default
  //  /// The encoding is Encoding.Default
  //  /// </summary>
  //  /// <param name="source">Source string to encrypt</param>
  //  /// <param name="key">SecurityKey</param>
  //  /// <returns>Encrypted string in base64 mode</returns>
  //  public static string EncryptTo3DESBase64(this string source, TSymmetricSecurityKey key) {
  //    return EncryptTo3DESBase64(source, key, Encoding.Default);
  //  }

  //  /// <summary>
  //  /// Encrypts a string in 3DES mode and Encoding.Default
  //  /// The encoding is Encoding.Default
  //  /// </summary>
  //  /// <param name="source">Source string to encrypt</param>
  //  /// <param name="keyType">SecurityKey type</param>
  //  /// <returns>Encrypted string in base64 mode</returns>
  //  public static string EncryptTo3DESBase64(this string source, TSymmetricSecurityKey.KeyType keyType) {
  //    return EncryptTo3DESBase64(source, new TSymmetricSecurityKey(keyType), Encoding.Default);
  //  }

  //  /// <summary>
  //  /// Encrypts a string in 3DES mode
  //  /// </summary>
  //  /// <param name="source">Source string to encrypt</param>
  //  /// <param name="key">SecurityKeys</param>
  //  /// <param name="encoding">The encoding used in the source string</param>
  //  /// <returns>Encrypted string in base64 mode</returns>
  //  public static string EncryptTo3DESBase64(this string source, TSymmetricSecurityKey key, Encoding encoding) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.EncryptTo3DESBase64(source, key, encoding);
  //  }

  //  /// <summary>
  //  /// Encrypts a string in 3DES mode
  //  /// </summary>
  //  /// <param name="source">Source string to encrypt</param>
  //  /// <param name="keyType">The type of security keys to use</param>
  //  /// <param name="encoding">The encoding used in the source string</param>
  //  /// <returns>Encrypted string in base64 mode</returns>
  //  public static string EncryptTo3DESBase64(this string source, TSymmetricSecurityKey.KeyType keyType, Encoding encoding) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.EncryptTo3DESBase64(source, new TSymmetricSecurityKey(keyType), encoding);
  //  }
  //  #endregion >> Encryption <<

  //  #region >> Decryption <<
  //  /// <summary>
  //  /// Decrypt a base64 string using 3DES and KeyType.Pwd
  //  /// </summary>
  //  /// <param name="source">The source string in base64</param>
  //  /// <returns>The decrypted string</returns>
  //  public static string DecryptFrom3DESBase64(this string source) {
  //    return DecryptFrom3DESBase64(source, new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Default));
  //  }

  //  /// <summary>
  //  /// Decrypt a base64 string using 3DES and Encoding.Default
  //  /// </summary>
  //  /// <param name="source">The source string in base64</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <returns>A decrypted string</returns>
  //  public static string DecryptFrom3DESBase64(this string source, TSymmetricSecurityKey key) {
  //    return DecryptFrom3DESBase64(source, key, Encoding.Default);
  //  }

  //  /// <summary>
  //  /// Decrypt a base64 string using 3DES and Encoding.Default
  //  /// </summary>
  //  /// <param name="source">The source string in base64</param>
  //  /// <param name="keyType">The type of security keys to use</param>
  //  /// <returns>A decrypted string</returns>
  //  public static string DecryptFrom3DESBase64(this string source, TSymmetricSecurityKey.KeyType keyType) {
  //    return DecryptFrom3DESBase64(source, new TSymmetricSecurityKey(keyType), Encoding.Default);
  //  }

  //  /// <summary>
  //  /// Decrypt a base64 string using 3DES
  //  /// </summary>
  //  /// <param name="source">The source string in base64</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <param name="encoding">The encoding used in the source string</param>
  //  /// <returns>The decrypted string</returns>
  //  public static string DecryptFrom3DESBase64(this string source, TSymmetricSecurityKey key, Encoding encoding) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.DecryptFrom3DESBase64(source, key, encoding);
  //  }
   
  //  /// <summary>
  //  /// Decrypt a base64 string using 3DES
  //  /// </summary>
  //  /// <param name="source">The source string in base64</param>
  //  /// <param name="keyType">The type of security keys to use</param>
  //  /// <param name="encoding">The encoding used in the source string</param>
  //  /// <returns>The decrypted string</returns>
  //  public static string DecryptFrom3DESBase64(this string source, TSymmetricSecurityKey.KeyType keyType, Encoding encoding) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.DecryptFrom3DESBase64(source, new TSymmetricSecurityKey(keyType), encoding);
  //  }
  //  #endregion >> Decryption <<

  //  #region >> Encrypt to file <<
  //  /// <summary>
  //  /// Encrypt a string using 3DES and KeyType.Pwd and Encoding.Default and store it into a file.
  //  /// The file is created and must NOT exist before.
  //  /// </summary>
  //  /// <param name="source">The string to encrypt</param>
  //  /// <param name="fileName">The full filename to create</param>
  //  public static void EncryptStringToFile(this string source, string fileName) {
  //    EncryptStringToFile(source, fileName, new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Default));
  //  }

  //  /// <summary>
  //  /// Encrypt a string using 3DES and Encoding.Default and store it into a file.
  //  /// The file is created and must NOT exist before.
  //  /// </summary>
  //  /// <param name="source">The string to encrypt</param>
  //  /// <param name="fileName">The full filename to create</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  public static void EncryptStringToFile(this string source, string fileName, TSymmetricSecurityKey key) {
  //    EncryptStringToFile(source, fileName, key, Encoding.Default);
  //  }

  //  /// <summary>
  //  /// Encrypt a string using 3DES and Encoding.Default and store it into a file.
  //  /// The file is created and must NOT exist before.
  //  /// </summary>
  //  /// <param name="source">The string to encrypt</param>
  //  /// <param name="fileName">The full filename to create</param>
  //  /// <param name="keyType">The type of security keys to use</param>
  //  public static void EncryptStringToFile(this string source, string fileName, TSymmetricSecurityKey.KeyType keyType) {
  //    EncryptStringToFile(source, fileName, new TSymmetricSecurityKey(keyType), Encoding.Default);
  //  }

  //  /// <summary>
  //  /// Encrypt a string using 3DES and store it into a file.
  //  /// The file is created and must NOT exist before.
  //  /// </summary>
  //  /// <param name="source">The string to encrypt</param>
  //  /// <param name="fileName">The full filename to create</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <param name="encoding">The encoding used in the source string</param>
  //  public static void EncryptStringToFile(this string source, string fileName, TSymmetricSecurityKey key, Encoding encoding) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    CurrentEncryptor.EncryptStringToFile(source, fileName, key, encoding);
  //  }

  //  /// <summary>
  //  /// Encrypt a string using 3DES and store it into a file.
  //  /// The file is created and must NOT exist before.
  //  /// </summary>
  //  /// <param name="source">The string to encrypt</param>
  //  /// <param name="fileName">The full filename to create</param>
  //  /// <param name="keyType">The type of security keys to use</param>
  //  /// <param name="encoding">The encoding used in the source string</param>
  //  public static void EncryptStringToFile(this string source, string fileName, TSymmetricSecurityKey.KeyType keyType, Encoding encoding) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    CurrentEncryptor.EncryptStringToFile(source, fileName, new TSymmetricSecurityKey(keyType), encoding);
  //  }
  //  #endregion >> Encrypt to file <<

  //  #region >> Decrypt from file <<
  //  /// <summary>
  //  /// Decrypt the content of a file into a string using 3DES and KeyType.Pwd and Encoding.Default
  //  /// Warning: Encrypted content should be a textual value, not a binary
  //  /// </summary>
  //  /// <param name="fileName">The full filename to decrypt</param>
  //  /// <returns>A string containing the decrypted content of the file</returns>
  //  public static string DecryptFileToString(this string fileName) {
  //    return DecryptFileToString(fileName, new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Default));
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a string using 3DES and Encoding.Default
  //  /// Warning: Encrypted content should be a textual value, not a binary
  //  /// </summary>
  //  /// <param name="fileName">The full filename to decrypt</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <returns>A string containing the decrypted content of the file</returns>
  //  public static string DecryptFileToString(this string fileName, TSymmetricSecurityKey key) {
  //    return DecryptFileToString(fileName, key, Encoding.Default);
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a string using 3DES and Encoding.Default
  //  /// Warning: Encrypted content should be a textual value, not a binary
  //  /// </summary>
  //  /// <param name="fileName">The full filename to decrypt</param>
  //  /// <param name="keyType">The type of security keys to use</param>
  //  /// <returns>A string containing the decrypted content of the file</returns>
  //  public static string DecryptFileToString(this string fileName, TSymmetricSecurityKey.KeyType keyType) {
  //    return DecryptFileToString(fileName, new TSymmetricSecurityKey(keyType), Encoding.Default);
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a string using 3DES
  //  /// Warning: Encrypted content should be a textual value, not a binary
  //  /// </summary>
  //  /// <param name="fileName">The full filename to decrypt</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <param name="encoding">The encoding used at the file creation</param>
  //  /// <returns>A string containing the decrypted content of the file</returns>
  //  public static string DecryptFileToString(this string fileName, TSymmetricSecurityKey key, Encoding encoding) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.DecryptFileToString(fileName, key, encoding);
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a string using 3DES
  //  /// Warning: Encrypted content should be a textual value, not a binary
  //  /// </summary>
  //  /// <param name="fileName">The full filename to decrypt</param>
  //  /// <param name="keyType">The type of security keys to use</param>
  //  /// <param name="encoding">The encoding used at the file creation</param>
  //  /// <returns>A string containing the decrypted content of the file</returns>
  //  public static string DecryptFileToString(this string fileName, TSymmetricSecurityKey.KeyType keyType, Encoding encoding) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.DecryptFileToString(fileName, new TSymmetricSecurityKey(keyType), encoding);
  //  }


  //  /// <summary>
  //  /// Decrypt the content of a file into a string using 3DES and KeyType.Pwd and Encoding.Default
  //  /// Warning: Encrypted content should be a textual value, not a binary
  //  /// </summary>
  //  /// <param name="fileInfo">The FileInfo of the filename to decrypt</param>
  //  public static string DecryptFileToString(this FileInfo fileInfo) {
  //    return DecryptFileToString(fileInfo, new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Default));
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a string using 3DES and Encoding.Default
  //  /// Warning: Encrypted content should be a textual value, not a binary
  //  /// </summary>
  //  /// <param name="fileInfo">The FileInfo of the filename to decrypt</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  public static string DecryptFileToString(this FileInfo fileInfo, TSymmetricSecurityKey key) {
  //    return DecryptFileToString(fileInfo, key, Encoding.Default);
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a string using 3DES and Encoding.Default
  //  /// Warning: Encrypted content should be a textual value, not a binary
  //  /// </summary>
  //  /// <param name="fileInfo">The FileInfo of the filename to decrypt</param>
  //  /// <param name="keyType">The type of security keys to use</param>
  //  public static string DecryptFileToString(this FileInfo fileInfo, TSymmetricSecurityKey.KeyType keyType) {
  //    return DecryptFileToString(fileInfo, new TSymmetricSecurityKey(keyType), Encoding.Default);
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a string using 3DES
  //  /// Warning: Encrypted content should be a textual value, not a binary
  //  /// </summary>
  //  /// <param name="fileInfo">The FileInfo of the filename to decrypt</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <param name="encoding">The encoding used at the file creation</param>
  //  /// <returns>A string containg the decrypted content of the file</returns>
  //  public static string DecryptFileToString(this FileInfo fileInfo, TSymmetricSecurityKey key, Encoding encoding) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.DecryptFileToString(fileInfo, key, encoding);
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a string using 3DES
  //  /// Warning: Encrypted content should be a textual value, not a binary
  //  /// </summary>
  //  /// <param name="fileInfo">The FileInfo of the filename to decrypt</param>
  //  /// <param name="keyType">The type of security keys to use</param>
  //  /// <param name="encoding">The encoding used at the file creation</param>
  //  /// <returns>A string containg the decrypted content of the file</returns>
  //  public static string DecryptFileToString(this FileInfo fileInfo, TSymmetricSecurityKey.KeyType keyType, Encoding encoding) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.DecryptFileToString(fileInfo, new TSymmetricSecurityKey(keyType), encoding);
  //  }
  //  #endregion >> Decrypt from file <<

  //  #endregion >> Extension methods for strings <<

  //  #region >> Extension methods for byte[] <<

  //  #region >> Encryption <<
  //  /// <summary>
  //  /// Encrypts a byte[] in 3DES mode using KeyType.Default
  //  /// </summary>
  //  /// <param name="source">Source byte[] to encrypt</param>
  //  /// <returns>Encrypted string in base64 mode or null if error</returns>
  //  public static string EncryptTo3DESBase64(this byte[] source) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.EncryptTo3DESBase64(source, new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Default));
  //  }

  //  /// <summary>
  //  /// Encrypts a byte[] in 3DES mode
  //  /// </summary>
  //  /// <param name="source">Source byte[] to encrypt</param>
  //  /// <param name="key">SecurityKeys</param>
  //  /// <returns>Encrypted string in base64 mode</returns>
  //  public static string EncryptTo3DESBase64(this byte[] source, TSymmetricSecurityKey key) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.EncryptTo3DESBase64(source, key);
  //  }

  //  /// <summary>
  //  /// Encrypts a string in 3DES mode
  //  /// </summary>
  //  /// <param name="source">Source byte[] to encrypt</param>
  //  /// <param name="keyType">The type of security keys to use</param>
  //  /// <returns>Encrypted string in base64 mode</returns>
  //  public static string EncryptTo3DESBase64(this byte[] source, TSymmetricSecurityKey.KeyType keyType) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.EncryptTo3DESBase64(source, new TSymmetricSecurityKey(keyType));
  //  }
  //  #endregion >> Encryption <<

  //  #region >> Decryption <<
  //  /// <summary>
  //  /// Decrypt a base64 string using 3DES and KeyType.Pwd
  //  /// </summary>
  //  /// <param name="source">The source string in base64</param>
  //  /// <returns>The decrypted string</returns>
  //  public static byte[] DecryptFrom3DESBase64ToBytes(this string source) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.DecryptFrom3DESBase64(source, new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Default));
  //  }

  //  /// <summary>
  //  /// Decrypt a base64 string encoded using 3DES
  //  /// </summary>
  //  /// <param name="source">The source string in base64</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <returns>A decrypted string</returns>
  //  public static byte[] DecryptFrom3DESBase64ToBytes(this string source, TSymmetricSecurityKey key) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.DecryptFrom3DESBase64(source, key);
  //  }

  //  /// <summary>
  //  /// Decrypt a base64 string using 3DES and Encoding.Default
  //  /// </summary>
  //  /// <param name="source">The source string in base64</param>
  //  /// <param name="keyType">The type of security keys to use</param>
  //  /// <returns>A decrypted string</returns>
  //  public static byte[] DecryptFrom3DESBase64ToBytes(this string source, TSymmetricSecurityKey.KeyType keyType) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.DecryptFrom3DESBase64(source, new TSymmetricSecurityKey(keyType));
  //  }
  //  #endregion >> Decryption <<

  //  #region >> Encrypt to file <<
  //  /// <summary>
  //  /// Encrypt a byte[] using 3DES and KeyType.Default and store it into a file.
  //  /// The file is created and must NOT exist before.
  //  /// </summary>
  //  /// <param name="source">The byte[] to encrypt</param>
  //  /// <param name="fileName">The full filename to create</param>
  //  public static void EncryptBytesToFile(this byte[] source, string fileName) {
  //    EncryptBytesToFile(source, fileName, TSymmetricSecurityKey.KeyType.Default);
  //  }

  //  /// <summary>
  //  /// Encrypt a byte[] using 3DES and store it into a file.
  //  /// The file is created and must NOT exist before.
  //  /// </summary>
  //  /// <param name="source">The byte[] to encrypt</param>
  //  /// <param name="fileName">The full filename to create</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  public static void EncryptBytesToFile(this byte[] source, string fileName, TSymmetricSecurityKey key) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    CurrentEncryptor.EncryptBytesToFile(source, fileName, key);
  //  }

  //  /// <summary>
  //  /// Encrypt a byte[] using 3DES and store it into a file.
  //  /// The file is created and must NOT exist before.
  //  /// </summary>
  //  /// <param name="source">The byte[] to encrypt</param>
  //  /// <param name="fileName">The full filename to create</param>
  //  /// <param name="keyType">The type of security keys to use</param>
  //  public static void EncryptBytesToFile(this byte[] source, string fileName, TSymmetricSecurityKey.KeyType keyType) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    CurrentEncryptor.EncryptBytesToFile(source, fileName, new TSymmetricSecurityKey(keyType));
  //  }
  //  #endregion >> Encrypt to file <<

  //  #region >> Decrypt from file <<
  //  /// <summary>
  //  /// Decrypt the content of a file into a byte[] using 3DES and KeyType.Default
  //  /// </summary>
  //  /// <param name="fileName">The full filename to decrypt</param>
  //  /// <returns>A string containing the decrypted content of the file</returns>
  //  public static byte[] DecryptFileToBytes(this string fileName) {
  //    return DecryptFileToBytes(fileName, new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Default));
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a byte[] using 3DES
  //  /// </summary>
  //  /// <param name="fileName">The full filename to decrypt</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  /// <returns>A byte[] containing the decrypted content of the file</returns>
  //  public static byte[] DecryptFileToBytes(this string fileName, TSymmetricSecurityKey key) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.DecryptFileToBytes(fileName, key);
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a byte[] using 3DES
  //  /// </summary>
  //  /// <param name="fileInfo">The FileInfo of the filename to decrypt</param>
  //  /// <param name="keyType">The type of security keys to use</param>
  //  /// <returns>A string containg the decrypted content of the file</returns>
  //  public static byte[] DecryptFileToBytes(this string fileName, TSymmetricSecurityKey.KeyType keyType) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.DecryptFileToBytes(fileName, new TSymmetricSecurityKey(keyType));
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a byte[] using 3DES and KeyType.Default
  //  /// </summary>
  //  /// <param name="fileInfo">The FileInfo of the filename to decrypt</param>
  //  public static byte[] DecryptFileToBytes(this FileInfo fileInfo) {
  //    return DecryptFileToBytes(fileInfo, new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Default));
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a byte[] using 3DES
  //  /// </summary>
  //  /// <param name="fileInfo">The FileInfo of the filename to decrypt</param>
  //  /// <param name="key">The SecurityKeys</param>
  //  public static byte[] DecryptFileToBytes(this FileInfo fileInfo, TSymmetricSecurityKey key) {
  //    using (TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor()) {
  //      return CurrentEncryptor.DecryptFileToBytes(fileInfo.FullName, key);
  //    }
  //  }

  //  /// <summary>
  //  /// Decrypt the content of a file into a byte[] using 3DES
  //  /// </summary>
  //  /// <param name="fileInfo">The FileInfo of the filename to decrypt</param>
  //  /// <param name="keyType">The type of security keys to use</param>
  //  /// <returns>A string containg the decrypted content of the file</returns>
  //  public static byte[] DecryptFileToBytes(this FileInfo fileInfo, TSymmetricSecurityKey.KeyType keyType) {
  //    TSymmetricEncryptor CurrentEncryptor = new TSymmetricEncryptor();
  //    return CurrentEncryptor.DecryptFileToBytes(fileInfo.FullName, new TSymmetricSecurityKey(keyType));
  //  }
  //  #endregion >> Decrypt from file <<

  //  #endregion >> Extension methods for byte[] <<


  //}

}
