using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Encryption {
  public enum THashingMethods {
    MD5,
    SHA1,
    SHA256,
    SHA384,
    SHA512,
    RIPEMD160
  }

  public static class StringExtensions {

    public static string HashToBase64(this string source, THashingMethods hashMethod = THashingMethods.SHA256) {
      switch (hashMethod) {
        case THashingMethods.MD5:
          using (MD5CryptoServiceProvider MD5Hasher = new MD5CryptoServiceProvider()) {
            byte[] HashedData = MD5Hasher.ComputeHash(Encoding.Default.GetBytes(source));
            return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);
          }
        case THashingMethods.SHA1:
          using (SHA1CryptoServiceProvider SHA1Hasher = new SHA1CryptoServiceProvider()) {
            byte[] HashedData = SHA1Hasher.ComputeHash(Encoding.Default.GetBytes(source));
            return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);
          }
        case THashingMethods.SHA256:
          using (SHA256CryptoServiceProvider SHA256Hasher = new SHA256CryptoServiceProvider()) {
            byte[] HashedData = SHA256Hasher.ComputeHash(Encoding.Default.GetBytes(source));
            return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);
          }
        case THashingMethods.SHA384:
          using (SHA384CryptoServiceProvider SHA384Hasher = new SHA384CryptoServiceProvider()) {
            byte[] HashedData = SHA384Hasher.ComputeHash(Encoding.Default.GetBytes(source));
            return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);
          }
        case THashingMethods.SHA512:
          using (SHA512CryptoServiceProvider SHA512Hasher = new SHA512CryptoServiceProvider()) {
            byte[] HashedData = SHA512Hasher.ComputeHash(Encoding.Default.GetBytes(source));
            return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);
          }
        case THashingMethods.RIPEMD160:
          using (RIPEMD160Managed RIPEMD160Hasher = new RIPEMD160Managed()) {
            byte[] HashedData = RIPEMD160Hasher.ComputeHash(Encoding.Default.GetBytes(source));
            return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);
          }
        default:
          return "";
      }
    }
    public static bool VerifyHashFromBase64(this string source, string base64Hash, THashingMethods hashMethod = THashingMethods.SHA256) {
      string HashToTest = source.HashToBase64(hashMethod);
      return (HashToTest == base64Hash);
    }

    public static string HMacToBase64(this string source, string key, THashingMethods hashMethod = THashingMethods.SHA256) {
      return source.HMacToBase64(Encoding.Default.GetBytes(key), hashMethod);
    }
    public static string HMacToBase64(this string source, byte[] key, THashingMethods hashMethod = THashingMethods.SHA256) {
      switch (hashMethod) {
        case THashingMethods.MD5:
          using (HMACMD5 HMACMD5Hasher = new HMACMD5(key)) {
            byte[] HashedData = HMACMD5Hasher.ComputeHash(Encoding.Default.GetBytes(source));
            return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);
          }
        case THashingMethods.SHA1:
          using (HMACSHA1 HMACSHA1Hasher = new HMACSHA1(key)) {
            byte[] HashedData = HMACSHA1Hasher.ComputeHash(Encoding.Default.GetBytes(source));
            return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);
          }
        case THashingMethods.SHA256:
          using (HMACSHA256 HMACSHA256Hasher = new HMACSHA256(key)) {
            byte[] HashedData = HMACSHA256Hasher.ComputeHash(Encoding.Default.GetBytes(source));
            return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);
          }
        case THashingMethods.SHA384:
          using (HMACSHA384 HMACSHA384Hasher = new HMACSHA384(key)) {
            byte[] HashedData = HMACSHA384Hasher.ComputeHash(Encoding.Default.GetBytes(source));
            return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);
          }
        case THashingMethods.SHA512:
          using (HMACSHA512 HMACSHA512Hasher = new HMACSHA512(key)) {
            byte[] HashedData = HMACSHA512Hasher.ComputeHash(Encoding.Default.GetBytes(source));
            return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);
          }
        case THashingMethods.RIPEMD160:
          using (HMACRIPEMD160 HMACRIPEMD160Hasher = new HMACRIPEMD160(key)) {
            byte[] HashedData = HMACRIPEMD160Hasher.ComputeHash(Encoding.Default.GetBytes(source));
            return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);
          }
        default:
          return "";
      }
    }

    public static bool VerifyHMACFromBase64(this string source, string key, string base64Hash, THashingMethods hashMethod = THashingMethods.SHA256) {
      string HashToTest = source.HMacToBase64(key, hashMethod);
      return (HashToTest == base64Hash);
    }
    public static bool VerifyHMACFromBase64(this string source, byte[] key, string base64Hash, THashingMethods hashMethod = THashingMethods.SHA256) {
      string HashToTest = source.HMacToBase64(key, hashMethod);
      return (HashToTest == base64Hash);
    }
    

  }
}
