using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;

namespace BLTools.Encryption {
  public class TRSAPublicKey : IToXml{

    #region Public properties
    public string Name { get; set; }
    public string Key { get; set; }
    public string Filename {
      get {
        return string.Format("{0}-pub.blkey", Name);
      }
    } 
    #endregion Public properties

    #region Constructor(s)
    public TRSAPublicKey() {
      Name = "";
      Key = "";
    }

    public TRSAPublicKey(string name, string key = "") {
      Name = name;
      Key = key;
    } 
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("Public Key: {0}", Key);
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement("public");
      RetVal.SetAttributeValue("key", Key);
      return RetVal;
    } 
    #endregion Converters

    #region Public methods
    public void Save(string pathname) {
      string FullName = Path.Combine(pathname, Filename);
      try {
        XDocument XmlPublicKeyFile = new XDocument();
        XmlPublicKeyFile.Declaration = new XDeclaration("1.0", "UTF-8", "yes");
        XmlPublicKeyFile.Add(new XElement("root"));
        XmlPublicKeyFile.Root.Add(this.ToXml());
        TextWriter XmlWriter = new StreamWriter(FullName, false, Encoding.UTF8);
        XmlPublicKeyFile.Save(XmlWriter, SaveOptions.None);
        XmlWriter.Close();
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while saving public key : {0}", ex.Message), Severity.Error);
      }
    }

    public void Load(string pathname) {
      string FullName = Path.Combine(pathname, Filename);
      try {
        XDocument XmlPrivateKeyFile = XDocument.Load(FullName);
        XElement Root = XmlPrivateKeyFile.Root;
        Key = Root.Element("public").SafeReadAttribute<string>("key", "");
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading public key : {0}", ex.Message), Severity.Error);
      }
    } 
    #endregion Public methods

    #region Public static methods
    public static TRSAPublicKey Load(string name, string pathname) {
      TRSAPublicKey RetVal = new TRSAPublicKey(name);
      RetVal.Load(pathname);
      return RetVal;
    }
    #endregion Public static methods
  }
}
