using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;

namespace BLTools.AD {
  public class TSamGroup {

    #region Public properties
    public string Name { get; set; }
    public string Description { get; set; }
    #endregion Public properties

    #region Constructors
    public TSamGroup(string groupName, string description = "") {
      Name = groupName;
      Description = description;
    }
    #endregion Constructors

    #region Public methods
    public bool Exists() {
      try {
        GroupPrincipal CurrentGroup = GroupPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        return (CurrentGroup != null);
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error accessing directory while searching for group {0}: {1}", Name, Environment.MachineName, ex.Message), Severity.Error);
        return false;
      }
    }
    public bool Create() {
      try {
        GroupPrincipal NewGroup = new GroupPrincipal(new PrincipalContext(ContextType.Machine));
        NewGroup.Name = Name;
        NewGroup.Description = Description;
        NewGroup.Save();
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to create group {0} on computer {1}: {2}", Name, Environment.MachineName, ex.Message), Severity.Error);
        return false;
      }
    }
    public bool Delete() {
      try {
        GroupPrincipal CurrentGroup = GroupPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        CurrentGroup.Delete();
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to delete group {0} on computer {1}: {2}", Name, Environment.MachineName, ex.Message), Severity.Error);
        return false;
      }
    }
    public List<string> GetMembers() {
      List<string> RetVal = new List<string>();
      try {
        GroupPrincipal CurrentGroup = GroupPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        PrincipalSearchResult<Principal> Search = CurrentGroup.GetMembers();
        foreach (Principal PrincipalItem in Search) {
          if (PrincipalItem is UserPrincipal) {
            RetVal.Add(((UserPrincipal)PrincipalItem).SamAccountName);
          }
        }
        return RetVal;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error accessing directory while getting the userlist for group {0}: {1}", Name, Environment.MachineName, ex.Message), Severity.Error);
        return new List<string>();
      }
    }
    #endregion Public methods

  }
}
