using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BLTools.Security.Authorization {
  public class TSecurityUserIdCollection : List<string>, IToXml {
    public TSecurityUserIdCollection() {}

    public TSecurityUserIdCollection(TSecurityUserIdCollection UserIds) {
      foreach (string UserIdItem in UserIds) {
        Add(UserIdItem);
      }
    }

    public TSecurityUserIdCollection(IEnumerable<string> UserIds) {
      foreach (string UserIdItem in UserIds) {
        Add(UserIdItem);
      }
    }

    public TSecurityUserIdCollection(IEnumerable<XElement> UserIds) {
      foreach (XElement UserIdItem in UserIds) {
        Add(UserIdItem.SafeReadAttribute<string>("id"));
      }
    }

    public TSecurityUserIdCollection(XElement UserIds) {
      foreach (XElement UserIdItem in UserIds.Elements("userid")) {
        Add(UserIdItem.SafeReadAttribute<string>("id"));
      }
    }

    public XElement ToXml() {
      XElement RetVal = new XElement("userids");
      foreach (string UserIdItem in this) {
        RetVal.Add(new XElement("userid", new XAttribute("id", UserIdItem)));
      }
      return RetVal;
    }
  }
}
