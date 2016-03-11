using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace BLTools.Encryption {
  public class TRSAKeyPair : IToXml {

    public enum KeyTypeEnum {
      Unknown,
      User,
      Random
    }

    #region Public properties
    public string Name { get; set; }
    public KeyTypeEnum KeyType { get; set; }
    public int KeyLength { get; set; }
    public TRSAPrivateKey PrivateKey { get; set; }
    public TRSAPublicKey PublicKey { get; set; } 
    #endregion Public properties

    #region Constructor(s)
    public TRSAKeyPair() {
      Name = "";
      KeyType = KeyTypeEnum.Unknown;
      PublicKey = new TRSAPublicKey();
      PrivateKey = new TRSAPrivateKey();
      KeyLength = 1024;
    }

    public TRSAKeyPair(string keyname, KeyTypeEnum keyType, int keyLength = 1024, string publicKey = "", string privateKey = "") : this() {
      Name = keyname;
      KeyLength = keyLength;
      KeyType = keyType;
      switch (keyType) {
        case KeyTypeEnum.Random:
          using (RSACryptoServiceProvider RSACSP = new RSACryptoServiceProvider(keyLength)) {
            PublicKey = new TRSAPublicKey(Name, RSACSP.ToXmlString(false));
            PrivateKey = new TRSAPrivateKey(Name, RSACSP.ToXmlString(true));
          }
          break;
        case KeyTypeEnum.User:
          PublicKey = new TRSAPublicKey(Name, publicKey);
          PrivateKey = new TRSAPrivateKey(Name, privateKey);
          break;
      }
    }

    public TRSAKeyPair(string name) : this() {
      Name = name;
      PublicKey = new TRSAPublicKey(name);
      PrivateKey = new TRSAPrivateKey(name);
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendLine(PublicKey.ToString());
      RetVal.AppendLine(PrivateKey.ToString());
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement("RSAKeyPair");
      RetVal.Add(PrivateKey.ToXml());
      RetVal.Add(PublicKey.ToXml());
      return RetVal;
    }

    public RSAParameters ToPublicRSAParameters() {
      using (RSACryptoServiceProvider RSACSP = new RSACryptoServiceProvider()) {
        RSACSP.FromXmlString(PublicKey.Key);
        return RSACSP.ExportParameters(false);
      }
    }

    public RSAParameters ToPrivateRSAParameters() {
      using (RSACryptoServiceProvider RSACSP = new RSACryptoServiceProvider()) {
        RSACSP.FromXmlString(PrivateKey.Key);
        return RSACSP.ExportParameters(true);
      }
    } 
    #endregion Converters

    public void Save(string pathname) {
      PublicKey.Save(pathname);
      PrivateKey.Save(pathname);
    }

    public void Load(string pathname) {
      PublicKey.Load(pathname);
      PrivateKey.Load(pathname);
    }

    public static TRSAKeyPair Load(string keyname, string pathname) {
      TRSAKeyPair RetVal = new TRSAKeyPair(keyname);
      RetVal.Load(pathname);
      return RetVal;
    }
  }
}
