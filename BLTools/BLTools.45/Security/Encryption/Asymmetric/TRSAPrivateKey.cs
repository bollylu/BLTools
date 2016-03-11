using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace BLTools.Encryption {
  public class TRSAPrivateKey : IToXml {
    #region Public properties
    public string Name { get; set; }
    public string Key { get; set; }
    public string Filename {
      get {
        return string.Format("{0}-pvt.blkey", Name);
      }
    } 
    #endregion Public properties

    #region Constructor(s)
    public TRSAPrivateKey() {
      Name = "";
      Key = "";
    }

    public TRSAPrivateKey(string name, string key = "") {
      Name = name;
      Key = key;
    } 
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("Private Key: {0}", Key);
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement("private");
      RetVal.SetAttributeValue("key", Key);
      return RetVal;
    } 
    #endregion Converters

    #region Public methods
    public void Save(string pathname) {
      string FullName = Path.Combine(pathname, Filename);
      try {
        XDocument XmlPrivateKeyFile = new XDocument();
        XmlPrivateKeyFile.Declaration = new XDeclaration("1.0", "UTF-8", "yes");
        XmlPrivateKeyFile.Add(new XElement("root"));
        XmlPrivateKeyFile.Root.Add(this.ToXml());
        TextWriter XmlWriter = new StreamWriter(FullName, false, Encoding.UTF8);
        XmlPrivateKeyFile.Save(XmlWriter, SaveOptions.None);
        XmlWriter.Close();
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while saving private key : {0}", ex.Message), Severity.Error);
      }
    }

    public void Load(string pathname) {
      string FullName = Path.Combine(pathname, Filename);
      try {
        XDocument XmlPrivateKeyFile = XDocument.Load(FullName);
        XElement Root = XmlPrivateKeyFile.Root;
        Key = Root.Element("private").SafeReadAttribute<string>("key", "");
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading private key : {0}", ex.Message), Severity.Error);
      }
    } 
    #endregion Public methods

    #region Public static methods
    public static TRSAPrivateKey Load(string name, string pathname) {
      TRSAPrivateKey RetVal = new TRSAPrivateKey(name);
      RetVal.Load(pathname);
      return RetVal;
    } 
    #endregion Public static methods
  }
}
