using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.SQL {
  public partial class TSqlDatabase {

    #region Backup / restore
    public bool Backup(string backupFullName) {
      if (DebugMode) {
        Trace.WriteLine(string.Format("Backup request for database {0} : Destination : {1}", DatabaseName, backupFullName));
      }

      #region Validate backup path
      string BackupPath = Path.GetDirectoryName(backupFullName);
      try {
        if (DebugMode) {
          Trace.WriteLine(string.Format("Check for directory \"{0}\"", BackupPath));
        }
        if (!Directory.Exists(BackupPath)) {
          if (DebugMode) {
            Trace.WriteLine(string.Format("Directory \"{0}\" does not exist, it will be created", BackupPath));
          }
          Directory.CreateDirectory(BackupPath);
          if (DebugMode) {
            Trace.WriteLine("Done.");
          }
        }
      } catch (IOException ex) {
        Trace.WriteLine(string.Format("Error creating backup directory: {0}", ex.Message), Severity.Error);
        if (OnBackupCompleted != null) {
          OnBackupCompleted(this, new BoolEventArgs(false));
        }
        return false;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Generic exception creating backup directory : {0}\n{1}", ex.Message, ex.StackTrace), Severity.Error);
        if (OnBackupCompleted != null) {
          OnBackupCompleted(this, new BoolEventArgs(false));
        }
        return false;
      }
      #endregion Validate backup path

      #region Do the backup
      using (TSqlServer CurrentServer = new TSqlServer(ServerName, UserName, Password)) {
        try {
          BackupDeviceItem CurrentBackupDeviceItem = new BackupDeviceItem(backupFullName, DeviceType.File);

          Backup oBackup = new Backup();
          oBackup.Database = DatabaseName;
          oBackup.Action = BackupActionType.Database;
          oBackup.Devices.Add(CurrentBackupDeviceItem);
          oBackup.Initialize = true;


          Trace.WriteLine("Starting backup...");
          oBackup.SqlBackup(CurrentServer.SmoServer);
          Trace.WriteLine("Backup done.");

        } catch (Exception ex) {
          Trace.WriteLine(string.Format("Generic exception backing up database : {0}\n{1}", ex.Message, ex.StackTrace), Severity.Error);
          Trace.WriteLine(string.Format("  Inner exception : {0}", ex.InnerException.Message), Severity.Error);
          if (OnBackupCompleted != null) {
            OnBackupCompleted(this, new BoolEventArgs(false));
          }
          return false;
        }
      }
      #endregion Do the backup

      if (OnBackupCompleted != null) {
        OnBackupCompleted(this, new BoolEventArgs(true));
      }
      return true;
    }
    public void BackupAsync(string backupFullName) {
      Task.Factory.StartNew(() => Backup(backupFullName));
    }

    public bool Restore(string backupFullName) {
      return Restore(backupFullName, null);
    }
    public bool Restore(string backupFullName, string logicalDataName, string physicalDataFile, string logicalLogName, string physicalLogFile) {
      List<RelocateFile> RelocateFiles;
      RelocateFiles = new List<RelocateFile>();
      RelocateFiles.Add(new RelocateFile(logicalDataName, physicalDataFile));
      RelocateFiles.Add(new RelocateFile(logicalLogName, physicalLogFile));
      return Restore(backupFullName, RelocateFiles);
    }
    public bool Restore(string backupFullName, List<RelocateFile> relocateFiles) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(backupFullName)) {
        Trace.WriteLine(string.Format("Database \"{0}\" cannot be restored : specified backup device is null or empty", DatabaseName), Severity.Error);
        return false;
      }
      if (!File.Exists(backupFullName)) {
        Trace.WriteLine(string.Format("Database \"{0}\" cannot be restored : backup device \"{1}\" is missing or access is denied", DatabaseName, backupFullName), Severity.Error);
        return false;
      }
      #endregion Validate parameters

      if (DebugMode) {
        Trace.WriteLine(string.Format("Restore database {0} from backup device \"{1}\"", DatabaseName, backupFullName));
      }

      using (TSqlServer CurrentServer = new TSqlServer(ServerName, UserName, Password)) {
        try {
          BackupDeviceItem RestoreDevice = new BackupDeviceItem(Path.GetFullPath(backupFullName), DeviceType.File);
          Restore oRestore = new Restore();
          oRestore.Database = DatabaseName;
          oRestore.Action = RestoreActionType.Database;
          oRestore.Devices.Add(RestoreDevice);
          if (DebugMode) {
            DataTable FileList = oRestore.ReadFileList(CurrentServer.SmoServer);
            Trace.WriteLine(string.Format("  Available backup sets in backup device ({0}) :", FileList.Rows.Count));
            foreach (DataRow RowItem in FileList.Rows) {
              Trace.WriteLine(string.Format("    Database = {0}, Path = {1}", RowItem.Field<string>(0), RowItem.Field<string>(1)));
            }
          }
          if (relocateFiles != null) {
            foreach (RelocateFile RelocateFileItem in relocateFiles) {
              if (DebugMode) {
                Trace.WriteLine(string.Format("Relocate file \"{0}\" at location \"{1}\"", RelocateFileItem.LogicalFileName, RelocateFileItem.PhysicalFileName));
              }
              if (!Directory.Exists(Path.GetDirectoryName(RelocateFileItem.PhysicalFileName))) {
                Directory.CreateDirectory(Path.GetDirectoryName(RelocateFileItem.PhysicalFileName));
              }
            }
            oRestore.RelocateFiles.AddRange(relocateFiles);
          }
          oRestore.SqlRestore(CurrentServer.SmoServer);
          if (DebugMode) {
            Trace.WriteLine("Restore OK.");
          }
          if (OnRestoreCompleted != null) {
            OnRestoreCompleted(this, new BoolEventArgs(true));
          }
        } catch (Exception ex) {
          Trace.WriteLine(string.Format("Generic error restoring database : {0}", ex.Message), Severity.Error);
          Trace.Indent();
          Trace.WriteLine(string.Format("Inner exception : {0}", ex.InnerException.Message), Severity.Error);
          Trace.Unindent();
          if (OnRestoreCompleted != null) {
            OnRestoreCompleted(this, new BoolEventArgs(false));
          }
          return false;
        }
      }

      return true;
    }
    #endregion Backup / restore


  }
}
