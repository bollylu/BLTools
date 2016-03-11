using BLTools.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.XML {
  public class TXmlDatabase : IDatabase, IDisposable {

#if DEBUG
    public static bool DebugMode = true;
#else
    public static bool DebugMode = false;
#endif

    public static string DefaultServerName = ".";
    public static string DefaultDatabaseName = "";
    public static string DefaultUserName = "";
    public static string DefaultPassword = "";
    public static string DefaultTableExtension = "xml";

    /// <summary>
    /// Current Database server name
    /// </summary>
    public string ServerName {
      get {
        if (string.IsNullOrEmpty(_ServerName)) {
          return DefaultServerName;
        } else {
          return _ServerName;
        }
      }
      set {
        _ServerName = value ?? "";
      }
    }
    private string _ServerName;

    /// <summary>
    /// current database name
    /// </summary>
    public string DatabaseName {
      get {
        if (string.IsNullOrEmpty(_DatabaseName)) {
          return DefaultDatabaseName;
        } else {
          return _DatabaseName;
        }
      }
      set {
        _DatabaseName = value ?? "";
      }
    }
    private string _DatabaseName;

    /// <summary>
    /// Current user name (SQL identification)
    /// </summary>
    public string UserName {
      get {

        return _UserName ?? "";
      }
      set {
        _UserName = value ?? "";
      }
    }
    private string _UserName;

    /// <summary>
    /// Current user password (SQL identification)
    /// </summary>
    public string Password {
      get {
        return _Password ?? "";

      }
      set {
        _Password = value ?? "";
      }
    }
    private string _Password;

    /// <summary>
    /// Name of the database
    /// </summary>
    public string Name {
      get {
        return string.Format("{0}\\{1}", ServerName ?? "", DatabaseName ?? "");
      }
    }

    public string TableExtension {
      get {
        if (_TableExtension == null) {
          return DefaultTableExtension;
        } else {
          return _TableExtension;
        }
      }
    }
    private string _TableExtension;

    public string ConnectionString {
      get { throw new NotImplementedException(); }
    }

    #region Constructor(s)
    /// <summary>
    /// Builds a database object based on default values
    /// </summary>
    public TXmlDatabase()
      : this(DefaultServerName, DefaultDatabaseName, DefaultUserName, DefaultPassword) {
    }

    /// <summary>
    /// Builds a database object based on provided server, database, user and password.
    /// </summary>
    /// <remarks>Leaving username and password blank will use Integrated authentication. Otherwise, SQL authentication is used.</remarks>
    /// <param name="serverName">Name of the server to connect (SERVER or SERVER\INSTANCE). Use "(local)" for server on local machine</param>
    /// <param name="databaseName">Name of the database to open</param>
    /// <param name="userName">User name (blank = Windows authentication)</param>
    /// <param name="password">Paswword</param>
    public TXmlDatabase(string serverName, string databaseName, string userName="", string password="") {
      ServerName = serverName;
      DatabaseName = databaseName;
      UserName = userName;
      Password = password;
      if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(DatabaseName)) {
        if (DebugMode) {
          Trace.WriteLine(string.Format("Missing information for database ConnectionString: {0}", ConnectionString), Severity.Warning);
        }
      }
    }

    public TXmlDatabase(TXmlDatabase sqlDatabase) {
      ServerName = sqlDatabase.ServerName;
      DatabaseName = sqlDatabase.DatabaseName;
      UserName = sqlDatabase.UserName;
      Password = sqlDatabase.Password;
    }
    public virtual void Dispose() {
      //if (IsOpened) {
      //  TryClose();
      //}
    }
    #endregion Constructor(s)




    public bool TryOpen() {
      throw new NotImplementedException();
    }

    public void TryClose() {
      throw new NotImplementedException();
    }

    public bool IsOpened {
      get { throw new NotImplementedException(); }
    }

    public bool Exists() {
      string DbName = Path.Combine(ServerName, DatabaseName);
      bool DbExists = Directory.Exists(DbName);
      if (OnDatabaseExists != null) {
        OnDatabaseExists(this, new BoolAndMessageEventArgs(DbExists));
      }
      return DbExists;
    }

    public bool Create() {
      bool RetVal = false;
      string DbName = Path.Combine(ServerName, DatabaseName);
      if (!Directory.Exists(DbName)) {
        try {
          Trace.WriteLineIf(DebugMode,string.Format("Creating directory {0}", DbName));
          Directory.CreateDirectory(DbName);
          Trace.WriteLineIf(DebugMode, "Done.");
          RetVal = true;
        } catch (Exception ex) {
          Trace.WriteLine(string.Format("Failed to create directory {0} : {1}", DbName, ex.Message), Severity.Error);
        }
      }
      if (OnDatabaseCreated != null) {
        OnDatabaseCreated(this, new BoolAndMessageEventArgs(RetVal, string.Format("Database {0} created : {1}", DbName, RetVal)));
      }
      return RetVal;
    }

    public bool Drop(bool killConnections) {
      throw new NotImplementedException();
    }

    public bool TableExists(string table) {
      string TableName = Path.Combine(ServerName, DatabaseName, string.Format("{0}.XML", table));
      return File.Exists(TableName);
    }

    public IEnumerable<string> ListTables() {
      if (!Exists()) {
        yield break;
      }
      string DbName = Path.Combine(ServerName, DatabaseName);
      foreach (string TableItem in Directory.GetFiles(DbName, "*.XML")) {
        yield return Path.GetFileNameWithoutExtension(TableItem);
      }
    }

    public void CreateTables() {
      throw new NotImplementedException();
    }

    public void DropTables() {
      string DbName = Path.Combine(ServerName, DatabaseName);
      if (!Directory.Exists(DbName)) {
        return;
      }
      foreach (string TableItem in Directory.EnumerateFiles(DbName, string.Format("*.{0}", TableExtension))) {
        DropTable(TableItem);
      }
    }

    public void DropTables(IEnumerable<string> tables) {
      string DbName = Path.Combine(ServerName, DatabaseName);
      if (!Directory.Exists(DbName)) {
        return;
      }
      
    }

    public void DropTable(string table) {
      string DbName = Path.Combine(ServerName, DatabaseName);
      if (!Directory.Exists(DbName)) {
        return;
      }
      string TableName = Path.Combine(DbName, string.Format("{0}.{1}", table, TableExtension));
      if (!File.Exists(TableName)) {
        return;
      }
      try {
        Trace.WriteLine(string.Format("Dropping table {0}...", TableName));
        File.Delete(TableName);
        Trace.WriteLine("Done.");
      } catch (Exception ex) {
        Trace.WriteLineIf(DebugMode, string.Format("Unable to drop table {0} : {1}", TableName, ex.Message), Severity.Error);
      }
    }

    public System.Data.IDbTransaction StartTransaction() {
      throw new NotImplementedException();
    }

    public void CommitTransaction() {
      throw new NotImplementedException();
    }

    public void RollbackTransaction() {
      throw new NotImplementedException();
    }

    public bool Backup(string backupFullName) {
      throw new NotImplementedException();
    }

    public void BackupAsync(string backupFullName) {
      throw new NotImplementedException();
    }

    public bool Restore(string backupFullName) {
      throw new NotImplementedException();
    }

    public event EventHandler<BoolEventArgs> OnBackupCompleted;

    public event EventHandler<BoolEventArgs> OnRestoreCompleted;

    public event EventHandler<BoolAndMessageEventArgs> OnDatabaseAttached;

    public event EventHandler<BoolAndMessageEventArgs> OnDatabaseExists;

    public event EventHandler<BoolAndMessageEventArgs> OnDatabaseCreated;

    public event EventHandler<BoolAndMessageEventArgs> OnDatabaseDropped;

    public event EventHandler OnTransactionStarted;

    public event EventHandler OnTransactionCommit;

    public event EventHandler OnTransactionRollback;
  }
}
