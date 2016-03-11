using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BLTools.Security.Authorization {
  public class TSecurityUserCollection : List<TSecurityUser>, IToXml {

    #region Constructor(s)
    public TSecurityUserCollection() { }
    public TSecurityUserCollection(TSecurityUserCollection users) {
      foreach (TSecurityUser UserItem in users) {
        Add(new TSecurityUser(UserItem));
      }
    }

    public TSecurityUserCollection(IEnumerable<TSecurityUser> users) {
      foreach (TSecurityUser UserItem in users) {
        Add(new TSecurityUser(UserItem));
      }
    }

    public TSecurityUserCollection(XElement users) {
      foreach (XElement UserItem in users.Elements("user")) {
        Add(new TSecurityUser(UserItem));
      }
    }

    public TSecurityUserCollection(IEnumerable<XElement> users) {
      foreach (XElement UserItem in users) {
        Add(new TSecurityUser(UserItem));
      }
    } 
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      foreach (TSecurityUser UserItem in this) {
        RetVal.AppendLine(UserItem.ToString());
      }
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement("users");
      foreach (TSecurityUser UserItem in this) {
        RetVal.Add(UserItem.ToXml());
      }
      return RetVal;
    } 
    #endregion Converters

  }
}
