using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BLTools.Security.Authorization {
  public class TSecurityController {

    #region Public properties
    public TSecurityStorage Storage { get; private set; }
    public int LoggedInUsersCount {
      get {
        return LoggedInUsers.Count;
      }
    }
    #endregion Public properties

    #region Private variables
    private TSecurityUserIdCollection LoggedInUsers;
    #endregion Private variables

    #region Constructor(s)
    public TSecurityController(TSecurityStorage storage) {
      LoggedInUsers = new TSecurityUserIdCollection();
      Storage = storage;
    }
    #endregion Constructor(s)

    #region Public methods
    public bool IsLoggedOn(TSecurityUser user) {
      return LoggedInUsers.Exists(uid => uid == user.Id);
    }

    public bool LogOn(TSecurityUser user, string password = "") {
      try {
        TSecurityUser CurrentUser = Storage.Users.Find(u => u.Id == user.Id);
        if (CurrentUser == null) {
          Trace.WriteLine(string.Format("Logon attempt failed for user {0} : this user doesn't exist", user.ToString()));
          return false;
        }

        if (!CurrentUser.IsPasswordOk(password)) {
          Trace.WriteLine(string.Format("Logon attempt failed for user {0} : bad password", user.ToString()));
          return false;
        }

        if (!IsLoggedOn(CurrentUser)) {
          Trace.WriteLine(string.Format("Logon for user {0}", user.ToString()));
          LoggedInUsers.Add(user.Id);
          return true;
        }

        Trace.WriteLine(string.Format("Logon attempt for user {0}, but this user is already logged on.", user.ToString()));
        return true;
      } finally {
        Storage.Save();
      }

    }

    public void LogOff(TSecurityUser user) {
      if (IsLoggedOn(user)) {
        Trace.WriteLine(string.Format("LogOff for user {0}", user.ToString()));
        LoggedInUsers.Remove(user.Id);
      } else {
        Trace.WriteLine(string.Format("Logoff attemp for user {0}, but this user is already logged off.", user.ToString()));
      }
    }
    #endregion Public methods
  }
}
