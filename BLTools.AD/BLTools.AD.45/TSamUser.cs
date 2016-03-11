using System;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using BLTools;

namespace BLTools.AD {
  public class TSamUser {

    #region Public properties
    public string Name { get; set; }
    public string Description { get; set; }
    public string Password { get; set; }
    #endregion Public properties

    #region Constructors
    public TSamUser(string username, string description = "", string password = "") {
      Name = username;
      Description = description;
      Password = password;
    }
    #endregion Constructors

    #region Public methods
    public bool Exists() {
      try {
        UserPrincipal CurrentUser = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        return (CurrentUser != null);
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error accessing directory while searching for user {0} on {1}: {2}", Name, Environment.MachineName, ex.Message), Severity.Error);
        return false;
      }
    }
    public bool Create() {
      try {
        UserPrincipal NewUser = new UserPrincipal(new PrincipalContext(ContextType.Machine));
        NewUser.Name = Name;
        NewUser.Description = Description;
        NewUser.SetPassword(Password);
        NewUser.Save();
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to create user {0} on computer {1}: {2}", Name, Environment.MachineName, ex.Message), Severity.Error);
        return false;
      }
    }
    public bool SetPassword(string password) {
      try {
        UserPrincipal CurrentUser = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        CurrentUser.SetPassword(Password);
        CurrentUser.Save();
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to modify password for user {0} on computer {1}: {2}", Name, Environment.MachineName, ex.Message), Severity.Error);
        return false;
      }
    }
    public bool SetDescription(string description) {
      try {
        UserPrincipal CurrentUser = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        CurrentUser.Description = description;
        CurrentUser.Save();
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to modify password for user {0} on computer {1}: {2}", Name, Environment.MachineName, ex.Message), Severity.Error);
        return false;
      }
    }
    public bool Delete() {
      try {
        UserPrincipal CurrentUser = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        CurrentUser.Delete();
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to delete user {0} on computer {1}: {2}", Name, Environment.MachineName, ex.Message), Severity.Error);
        return false;
      }
    }

    public bool AddInGroup(string groupname) {
      try {
        GroupPrincipal CurrentGroup = GroupPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        if (CurrentGroup == null) {
          CurrentGroup = new GroupPrincipal(new PrincipalContext(ContextType.Machine));
          CurrentGroup.Name = groupname;
          CurrentGroup.Save();
        }
        UserPrincipal CurrentUser = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        if (CurrentUser.IsMemberOf(CurrentGroup)) {
          CurrentGroup.Members.Add(CurrentUser);
        }
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to add user {0} in group {1}: {2}", Name, groupname, ex.Message), Severity.Error);
        return false;
      }
    }

    public bool RemoveFromGroup(string groupname) {
      try {
        GroupPrincipal CurrentGroup = GroupPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        if (CurrentGroup == null) {
          throw new ApplicationException(string.Format("The specified group does not exist : {0}", groupname));
        }
        UserPrincipal CurrentUser = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        if (CurrentUser.IsMemberOf(CurrentGroup)) {
          CurrentGroup.Members.Remove(CurrentUser);
        } else {
          throw new ApplicationException(string.Format("The user {0} is not a member of the group {1}", Name, groupname));
        }
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to remove user {0} from group {1}: {2}", Name, groupname, ex.Message), Severity.Error);
        return false;
      }
    }
    public bool IsMemberOf(string groupname) {
      try {
        UserPrincipal CurrentUser = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        return CurrentUser.IsMemberOf(new GroupPrincipal(new PrincipalContext(ContextType.Machine), groupname));
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to verify whether user {0} is member of group {1} on computer {2}: {3}", Name, groupname, Environment.MachineName, ex.Message), Severity.Error);
        return false;
      }
    }

    public bool IsAccountLocked() {
      try {
        UserPrincipal CurrentUser = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        return CurrentUser.IsAccountLockedOut();
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to verify account {0} on computer {1}: {2}", Name, Environment.MachineName, ex.Message), Severity.Error);
        return false;
      }
    }

    public bool Unlock() {
      try {
        UserPrincipal CurrentUser = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        CurrentUser.UnlockAccount();
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to unlock account {0} on computer {1}: {2}", Name, Environment.MachineName, ex.Message), Severity.Error);
        return false;
      }
    }
    public bool SetPasswordNeverExpires(bool newValue) {
      try {
        UserPrincipal CurrentUser = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        CurrentUser.PasswordNeverExpires = newValue;
        CurrentUser.Save();
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to set PasswordNeverExpires for user {0} on computer {1}: {2}", Name, Environment.MachineName, ex.Message), Severity.Error);
        return false;
      }
    }
    public bool SetUserCannotChangePassword(bool newValue) {
      try {
        UserPrincipal CurrentUser = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        CurrentUser.UserCannotChangePassword = newValue;
        CurrentUser.Save();
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to set UserCannotChangePassword for user {0} on computer {1}: {2}", Name, Environment.MachineName, ex.Message), Severity.Error);
        return false;
      }
    }
    public bool SetAccountDisabled(bool newValue) {
      try {
        UserPrincipal CurrentUser = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Name);
        CurrentUser.Enabled = !newValue;
        CurrentUser.Save();
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to set Enabled for user {0} on computer {1}: {2}", Name, Environment.MachineName, ex.Message), Severity.Error);
        return false;
      }
    }
    #endregion Public methods

  }
}
