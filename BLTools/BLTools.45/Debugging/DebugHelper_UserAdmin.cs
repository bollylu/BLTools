using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Debugging {
  public static partial class ApplicationInfo {

    /// <summary>
    /// Indicates whether a domain user is local administrator
    /// </summary>
    /// <param name="domain">The domain name of the user</param>
    /// <param name="username">The name of the user</param>
    /// <returns>True if the user is local administrator</returns>
    public static bool IsUserAdmin(string domain, string username) {
      return IsUserAdmin(string.Format("{0}\\{1}", domain, username));
    }
    /// <summary>
    /// Indicates whether a local user is local administrator
    /// </summary>
    /// <param name="username">The name of the user</param>
    /// <returns>True if the user is local administrator</returns>
    public static bool IsUserAdmin(string username) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(username)) {
        return IsCurrentUserAdmin();
      }

      #endregion Validate parameters      WindowsIdentity UserIndentity;
      WindowsIdentity UserIndentity = new WindowsIdentity(username);
      WindowsPrincipal UserPrincipal = new WindowsPrincipal(UserIndentity);
      return UserPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
    }
    /// <summary>
    /// Indicates whether the current user is local administrator
    /// </summary>
    /// <returns>True if the user is local administrator</returns>
    public static bool IsCurrentUserAdmin() {
      WindowsIdentity UserIndentity = WindowsIdentity.GetCurrent();
      WindowsPrincipal UserPrincipal = new WindowsPrincipal(UserIndentity);
      return UserPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
    }
  }
}
