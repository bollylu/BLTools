using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BLTools.Encryption {
  /// <summary>
  /// Defines a Security Key (Secret Key and Initialization Vector)
  /// </summary>
  //public class TSymmetricSecurityKey : IDisposable {
  //  #region Enums
  //  /// <summary>
  //  /// Predefined SecurityKeys
  //  /// </summary>
  //  public enum KeyType {
  //    Unknown,
  //    Random,
  //    Default,
  //    User
  //  }
  //  /// <summary>
  //  /// List of the authorized key lengths
  //  /// </summary>
  //  public enum AuthorizedKeyLength {
  //    /// <summary>
  //    /// Unknown length
  //    /// </summary>
  //    Unknown = 0,
  //    /// <summary>
  //    /// Low encryption
  //    /// </summary>
  //    Low = 128,
  //    /// <summary>
  //    /// High encryption
  //    /// </summary>
  //    High = 192
  //  }
  //  #endregion Enums

  //  #region Public properties
  //  /// <summary>
  //  /// Name of the SecretKey
  //  /// </summary>
  //  public string Name { get; set; }

  //  /// <summary>
  //  /// The length of the secret key in bits
  //  /// </summary>
  //  public AuthorizedKeyLength KeyLength { get; set; }

  //  /// <summary>
  //  /// The secret key used for encryption/decryption.
  //  /// </summary>
  //  public byte[] SecretKey {
  //    get { return _SecretKey; }
  //    set {
  //      byte[] PaddingArray = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
  //      int KeyLengthInBytes = ((int)KeyLength / 8);
  //      if (value != null) {
  //        if (value.Length <= KeyLengthInBytes) {
  //          _SecretKey = new byte[KeyLengthInBytes];
  //          Array.Copy(value, _SecretKey, value.Length);
  //          Array.Copy(PaddingArray, 0, _SecretKey, value.Length, KeyLengthInBytes - value.Length);
  //        } else {
  //          _SecretKey = new byte[KeyLengthInBytes];
  //          Array.Copy(value, _SecretKey, KeyLengthInBytes);
  //        }
  //      }
  //    }
  //  }
  //  private byte[] _SecretKey;

  //  /// <summary>
  //  /// The secret initialization vector used for encryption/decryption.
  //  /// </summary>
  //  public byte[] InitializationVector { get; set; }
  //  #endregion Public properties

  //  private static Random SeedGenerator;

  //  #region Constructor(s)

  //  static TSymmetricSecurityKey() {
  //    SeedGenerator = new Random();
  //  }

  //  /// <summary>
  //  /// Initialize an empty SecurityKey
  //  /// </summary>
  //  public TSymmetricSecurityKey() : this(KeyType.Default) { }

  //  /// <summary>
  //  /// Initialize a SecurityKey with predefined values
  //  /// </summary>
  //  /// <param name="key"></param>
  //  public TSymmetricSecurityKey(KeyType key) {
  //    switch (key) {
  //      case KeyType.Unknown:
  //        KeyLength = AuthorizedKeyLength.Low;
  //        SecretKey = new byte[] { };
  //        InitializationVector = new byte[] { };
  //        Name = KeyType.Unknown.ToString();
  //        break;

  //      case KeyType.Random:
  //        KeyLength = AuthorizedKeyLength.High;
  //        Random RandomGenerator = new Random(SeedGenerator.Next());
  //        SecretKey = new byte[(int)KeyLength];
  //        RandomGenerator.NextBytes(SecretKey);
  //        InitializationVector = new byte[8];
  //        RandomGenerator.NextBytes(InitializationVector);
  //        Name = KeyType.Random.ToString();
  //        break;

  //      case KeyType.Default:
  //        KeyLength = AuthorizedKeyLength.High;
  //        SecretKey = Encoding.Default.GetBytes("this is my secret key...");
  //        InitializationVector = Encoding.Default.GetBytes("initialv");
  //        Name = KeyType.Default.ToString();
  //        break;

  //    }
  //  }

  //  public TSymmetricSecurityKey(string key) {
  //    #region Validate parameters
  //    if (key == null) {
  //      throw new ArgumentNullException("Error: cannot create a security key from a null key", "key");
  //    } 
  //    #endregion Validate parameters
  //    Name = "User";
  //    KeyLength = AuthorizedKeyLength.High;
  //    SecretKey = Encoding.Default.GetBytes(key.Left(24));
  //    InitializationVector = new byte[] { 1, 6, 7, 3, 4, 9, 2, 8 };
  //  }

  //  /// <summary>
  //  /// Cleanup
  //  /// </summary>
  //  public void Dispose() {
  //    KeyLength = AuthorizedKeyLength.Unknown;
  //    InitializationVector = null;
  //    SecretKey = null;
  //  }

  //  #endregion Constructor(s)

  //  #region Converters
  //  /// <summary>
  //  /// Returns a string representation of the object
  //  /// </summary>
  //  /// <returns>The name</returns>
  //  public override string ToString() {
  //    StringBuilder RetVal = new StringBuilder();
  //    RetVal.AppendFormat("Skey={0}", Name);
  //    RetVal.AppendFormat(", Secret={0}", SecretKey.ToHexString());
  //    RetVal.AppendFormat(", IV={0}", InitializationVector.ToHexString());
  //    return RetVal.ToString();
  //  }
  //  #endregion Converters



  //}
}
