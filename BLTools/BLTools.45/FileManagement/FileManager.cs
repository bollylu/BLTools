using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace BLTools.FileManagement {
  /// <summary>
  /// Gathers extended file version info
  /// </summary>
  public class FileManager {
    #region Constructor(s)
    /// <summary>
    /// Gathers extended file version info from a given folder, for a given pattern, optionally recursive through all sub-folders
    /// </summary>
    /// <param name="foldername">The source folder name</param>
    /// <param name="pattern">The pattern (default="*.*")</param>
    /// <param name="isRecursive">Do we recurse through sub-folders (default=true)</param>
    /// <returns>The extended file version infos</returns>
    public IEnumerable<ExtendedFileVersionInfo> GetFileVersionInfo(string foldername, string pattern = "*.*", bool isRecursive = true) {
      DirectoryInfo CurrentFolder;
      try {
        CurrentFolder = new DirectoryInfo(foldername);
      } catch (UnauthorizedAccessException) {
        yield break;
      }
      IEnumerable<FileInfo> Files;
      try {
        Files = CurrentFolder.GetFiles(pattern, isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
      } catch (UnauthorizedAccessException) {
        yield break;
      }
      foreach (FileInfo FileItem in Files) {
        ExtendedFileVersionInfo RetVal = new ExtendedFileVersionInfo(FileItem.FullName);
        yield return RetVal;
      }
    }
    #endregion Constructor(s)
  }

}
