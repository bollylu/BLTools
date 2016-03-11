using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BLTools.Security.Authorization {
  public class TSecurityGroupCollection : List<TSecurityGroup>, IToXml {

    public TSecurityGroupCollection () {}

    public TSecurityGroupCollection(TSecurityGroupCollection groups) {
      foreach(TSecurityGroup GroupItem in groups) {
        Add(new TSecurityGroup(GroupItem));
      }
    }

    public TSecurityGroupCollection(IEnumerable<TSecurityGroup> groups) {
      foreach (TSecurityGroup GroupItem in groups) {
        Add(new TSecurityGroup(GroupItem));
      }
    }

    public TSecurityGroupCollection(IEnumerable<XElement> groups) {
      foreach (XElement GroupItem in groups) {
        Add(new TSecurityGroup(GroupItem));
      }
    }

    public TSecurityGroupCollection(XElement groups) {
      foreach (XElement GroupItem in groups.Elements("group")) {
        Add(new TSecurityGroup(GroupItem));
      }
    }

    public XElement ToXml() {
      XElement RetVal = new XElement("groups");
      foreach (TSecurityGroup GroupItem in this) {
        RetVal.Add(GroupItem.ToXml());
      }
      return RetVal;
    }
  }
}
