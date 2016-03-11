using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml.Linq;
using System.IO;

namespace BLTools.Security.Authorization {
  public class TSecurityStorageXml : TSecurityStorage {
    
    #region Constructor(s)
    public TSecurityStorageXml() {}
    public TSecurityStorageXml(string name) {
      Name = name;
      Users = new TSecurityUserCollection();
      Groups = new TSecurityGroupCollection();
    } 
    #endregion Constructor(s)

    #region Public methods
    public override void Save() {
      Save(Name);
    }

    public override void Save(string name) {
      #region Validate parameters
      if (name == null) {
        string Msg = "Unable to save TSecurityStorage to a null filename";
        Trace.WriteLine(Msg, Severity.Error);
        throw new ArgumentNullException(Msg, "name");
      }
      if (name.Trim() == "") {
        string Msg = "Unable to save TSecurityStorage to an empty filename";
        Trace.WriteLine(Msg, Severity.Error);
        throw new ArgumentException(Msg, "name");
      }
      #endregion Validate parameters

      XDocument Destination = new XDocument();
      Destination.Declaration = new XDeclaration("1.0", Encoding.Default.EncodingName, "yes");
      Destination.Add(new XElement("Root"));
      Destination.Element("Root").Add(Users.ToXml());
      Destination.Element("Root").Add(Groups.ToXml());
      try {
        Destination.Save(name, SaveOptions.None);
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to read TSecurityStorage \"{0}\" : {1}", name, ex.Message));
      }

      if (OnSaved != null) {
        OnSaved(this, EventArgs.Empty);
      }
    }

    public void Load() {
      TSecurityStorageXml TempStorage = TSecurityStorageXml.Load(Name);
      Name = TempStorage.Name;
      Users = new TSecurityUserCollection(TempStorage.Users);
      Groups = new TSecurityGroupCollection(TempStorage.Groups);
      if (OnLoaded != null) {
        OnLoaded(this, EventArgs.Empty);
      }
    }

    public void Clear() {
      Users = new TSecurityUserCollection();
      Groups = new TSecurityGroupCollection();
    }
    #endregion Public methods

    #region Public static methods
    public static TSecurityStorageXml Load(string name) {
      #region Validate parameters
      if (name == null) {
        string Msg = "Unable to read TSecurityStorage from a null filename";
        Trace.WriteLine(Msg, Severity.Error);
        throw new ArgumentNullException(Msg, "name");
      }
      if (name.Trim() == "") {
        string Msg = "Unable to read TSecurityStorage from an empty filename";
        Trace.WriteLine(Msg, Severity.Error);
        throw new ArgumentException(Msg, "name");
      }
      if (!File.Exists(name)) {
        string Msg = "Unable to read TSecurityStorage : file is missing or access is denied";
        Trace.WriteLine(Msg, Severity.Error);
        throw new ArgumentException(Msg, "name");
      }
      #endregion Validate parameters
      TSecurityStorageXml RetVal = new TSecurityStorageXml(name);
      try {
        XDocument Source = XDocument.Load(name);
        XElement Root = Source.Root;
        RetVal.Users = new TSecurityUserCollection(Root.Element("users"));
        RetVal.Groups = new TSecurityGroupCollection(Root.Element("groups"));
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to read TSecurityStorage \"{0}\" : {1}", name, ex.Message));
        throw;
      }
      return RetVal;
    }
    #endregion Public static methods

    #region Events
    public event EventHandler OnSaved;
    public event EventHandler OnLoaded;
    #endregion Events
  }
}
