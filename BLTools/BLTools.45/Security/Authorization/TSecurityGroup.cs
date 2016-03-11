using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;

namespace BLTools.Security.Authorization {
  public class TSecurityGroup : IToXml {
    public string Id { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TSecurityUserIdCollection UserIds { get; set; }
    public TSecurityGroupCollection Groups { get; set; }

    public TSecurityGroup() {
      Id = "";
      Name = "";
      Description = "";
      UserIds = new TSecurityUserIdCollection();
      Groups = new TSecurityGroupCollection();
    }

    public TSecurityGroup(string name, string description = "")
      : this() {
      Name = name;
      Description = description;
    }

    public TSecurityGroup(TSecurityGroup group) {
      Id = group.Id;
      Name = group.Name;
      Description = group.Description;
      UserIds = new TSecurityUserIdCollection(group.UserIds);
      Groups = new TSecurityGroupCollection(group.Groups);
    }

    public TSecurityGroup(XElement group) {
      if (group == null) {
        string Msg = "Unable to create a TSecurityGroup from a null XElement";
        Trace.WriteLine(Msg, Severity.Error);
        throw new ArgumentNullException(Msg, "group");
      }
      Id = group.SafeReadAttribute<string>("id", "");
      Name = group.SafeReadAttribute<string>("name", "");
      Description = group.SafeReadElementValue<string>("description", "");
      UserIds = new TSecurityUserIdCollection(group.SafeReadElement("userids"));
      Groups = new TSecurityGroupCollection(group.SafeReadElement("groups"));
    }

    public XElement ToXml() {
      XElement RetVal = new XElement("group");
      RetVal.SetAttributeValue("id", Id);
      RetVal.SetAttributeValue("name", Name);
      RetVal.SetElementValue("description", Description);
      RetVal.Add(UserIds.ToXml());
      RetVal.Add(Groups.ToXml());
      return RetVal;
    }
  }
}
