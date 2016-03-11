using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml.Linq;

namespace BLTools.Security.Authorization {
  public class TSecurityUser : IToXml {

    #region Public properties
    public string Id { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string EncryptedPassword { get; set; }
    public DateTime LastSuccessfullLogon { get; private set; }
    public DateTime LastFailedLogon { get; private set; }
    #endregion Public properties

    #region Constructor(s)
    public TSecurityUser() {
      Id = Guid.NewGuid().ToString();
      Name = "";
      Description = "";
      EncryptedPassword = "";
    }

    public TSecurityUser(string name, string password = "", string description = "")
      : this() {
      Name = name;
      EncryptedPassword = password;
      Description = description;
    }

    public TSecurityUser(TSecurityUser user) {
      if (user == null) {
        string Msg = "Unable to create a TSecurityUser from a null TSecurityUser";
        Trace.WriteLine(Msg, Severity.Error);
        throw new ArgumentNullException(Msg, "user");
      }
      Id = user.Id;
      Name = user.Name;
      EncryptedPassword = user.EncryptedPassword;
      Description = user.Description;
    }

    public TSecurityUser(XElement user) {
      if (user == null) {
        string Msg = "Unable to create a TSecurityUser from a null XElement";
        Trace.WriteLine(Msg, Severity.Error);
        throw new ArgumentNullException(Msg, "user");
      }
      Id = user.SafeReadAttribute<string>("id", "");
      Name = user.SafeReadAttribute<string>("name", "");
      EncryptedPassword = user.SafeReadAttribute<string>("password", "");
      Description = user.SafeReadElementValue<string>("description", "");
    } 
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("User: \"{0}\"", Id);
      RetVal.AppendFormat(", Name: \"{0}\"", Name);
      RetVal.AppendFormat(", Password: \"{0}\"", EncryptedPassword);
      RetVal.AppendFormat(", Description: \"{0}\"", Description);
      return RetVal.ToString();
    }
    public XElement ToXml() {
      XElement RetVal = new XElement("user");
      RetVal.SetAttributeValue("id", Id);
      RetVal.SetAttributeValue("name", Name);
      RetVal.SetAttributeValue("password", EncryptedPassword);
      RetVal.SetElementValue("description", Description);
      return RetVal;
    } 
    #endregion Converters

    public bool IsPasswordOk(string password) {
      if (password == EncryptedPassword) {
        LastSuccessfullLogon = DateTime.Now;
        return true;
      } else {
        LastFailedLogon = DateTime.Now;
        return false;
      }
    }

  }
}
