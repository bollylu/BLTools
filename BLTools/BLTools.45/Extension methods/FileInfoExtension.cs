using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  /// <summary>
  /// Extensions for FileInfo and DirectoryInfo
  /// </summary>
  public static class FileInfoExtension {
    /// <summary>
    /// Indicates if the directory has Read permission for the current user
    /// </summary>
    /// <param name="directoryInfo">The directory to test</param>
    /// <returns>True if the current user can read the directory</returns>
    public static bool HaveReadPermission(this DirectoryInfo directoryInfo) {
      if (directoryInfo == null) {
        return false;
      }
      if (!Directory.Exists(directoryInfo.FullName)) {
        return false;
      }
      try {
        FileIOPermission Permission = new FileIOPermission(FileIOPermissionAccess.Read, directoryInfo.FullName);
        Permission.Demand();
        return true;
      } catch {
        return false;
      }
    }

    /// <summary>
    /// Indicates if the directory has Write permission for the current user
    /// </summary>
    /// <param name="directoryInfo">The directory to test</param>
    /// <returns>True if the current user can write the directory</returns>
    public static bool HaveWritePermission(this DirectoryInfo directoryInfo) {
      if (directoryInfo == null) {
        return false;
      }
      if (!Directory.Exists(directoryInfo.FullName)) {
        return false;
      }
      try {
        FileIOPermission Permission = new FileIOPermission(FileIOPermissionAccess.Write, directoryInfo.FullName);
        Permission.Demand();
        return true;
      } catch {
        return false;
      }
    }

    /// <summary>
    /// Indicates if the file has Read permission for the current user
    /// </summary>
    /// <param name="directoryInfo">The file to test</param>
    /// <returns>True if the current user can read the file</returns>
    public static bool HaveReadPermission(this FileInfo fileInfo) {
      if (fileInfo == null) {
        return false;
      }
      if (!Directory.Exists(fileInfo.FullName)) {
        return false;
      }
      try {
        FileIOPermission Permission = new FileIOPermission(FileIOPermissionAccess.Read, fileInfo.FullName);
        Permission.Demand();
        return true;
      } catch {
        return false;
      }
    }

    /// <summary>
    /// Indicates if the file has Write permission for the current user
    /// </summary>
    /// <param name="directoryInfo">The file to test</param>
    /// <returns>True if the current user can write the file</returns>
    public static bool HaveWritePermission(this FileInfo fileInfo) {
      if (fileInfo == null) {
        return false;
      }
      if (!Directory.Exists(fileInfo.FullName)) {
        return false;
      }
      try {
        FileIOPermission Permission = new FileIOPermission(FileIOPermissionAccess.Write, fileInfo.FullName);
        Permission.Demand();
        return true;
      } catch {
        return false;
      }
    }

  }

}
