using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using BLTools;
using BLTools.Data;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Threading.Tasks;

namespace BLTools.SQL {
  public class TSqlDatabase : IDisposable, IDatabase {

    #region Constants for connection string
    private const string CS_SERVERNAME = "server";
    private const string CS_DATABASENAME = "database";
    private const string CS_USERID = "user id";
    private const string CS_USERNAME = "username";
    private const string CS_TRUSTED_CONNECTION = "trusted_connection";
    private const string CS_PASSWORD = "password";
    private const string CS_USE_MARS = "multipleactiveresultsets";
    private const string CS_ATTACH_DBFILENAME = "attachdbfilename";
    private const string CS_USER_INSTANCE = "userinstance";
    #endregion Constants for connection string

    #region Default values
    public static string DEFAULT_SERVERNAME = "(local)";
    public static string DEFAULT_DATABASENAME = "master";
    public static string DEFAULT_USERNAME = "";
    public static string DEFAULT_PASSWORD = "";
    public static bool DEFAULT_USE_INTEGRATED_SECURITY = true;

    /// <summary>
    /// This value will be used later to set UsePooledConnections when no specific value is passed
    /// </summary>
    public static bool DefaultUsePooledConnections = false;
    /// <summary>
    /// This value will be used later to set UseMars when no specific value is passed
    /// </summary>
    public static bool DefaultUseMars = true;
    #endregion Default values

    #region Public properties
    /// <summary>
    /// When set to true, provide additional debug information
    /// </summary>
#if DEBUG
    public static bool DebugMode = true;
#else
    public static bool DebugMode = false;
#endif

    /// <summary>
    /// Name of the database
    /// </summary>
    public virtual string Name {
      get {
        return string.Format("{0}:{1}", ServerName ?? "", DatabaseName ?? "");
      }
    }
    /// <summary>
    /// Indicate whether the connection is opened or not
    /// </summary>
    public virtual bool IsOpened {
      get {
        if (Connection != null) {
          return Connection.State == ConnectionState.Open;
        } else {
          return false;
        }
      }
    }

    /// <summary>
    /// The underlying connection to the database
    /// </summary>
    public SqlConnection Connection { get; private set; }

    /// <summary>
    /// Returns a new SMO connection based on the current server and database parameters
    /// </summary>
    public virtual ServerConnection SmoConnection {
      get {
        return new ServerConnection(new SqlConnection(ConnectionString));
      }
    }

    #region Connection string parameters
    /// <summary>
    /// The timeout when trying to connect to the database (in seconds)
    /// </summary>
    /// <remarks>Max value is 1000. If 0, then 300.</remarks>
    public virtual int ConnectionTimeout {
      get {
        return _ConnectionTimeout;
      }
      set {
        if (value <= 0) {
          _ConnectionTimeout = 300;
        } else {
          _ConnectionTimeout = Math.Min(value, 1000);
        }
      }
    }

    /// <summary>
    /// True to use the Pooled Connections
    /// </summary>
    public bool UsePooledConnections { get; set; }

    /// <summary>
    /// True to use MARS. False otherwise.
    /// </summary>
    /// <remarks>Only available with SQL 2005 and up.</remarks>
    public bool UseMars { get; set; }

    /// <summary>
    /// Current Database server name
    /// </summary>
    public virtual string ServerName {
      get {
        if (string.IsNullOrEmpty(_ServerName)) {
          return DEFAULT_SERVERNAME;
        } else {
          return _ServerName;
        }
      }
      set {
        _ServerName = value ?? "";
      }
    }

    /// <summary>
    /// current database name
    /// </summary>
    public virtual string DatabaseName {
      get {
        if (string.IsNullOrEmpty(_DatabaseName)) {
          return DEFAULT_DATABASENAME;
        } else {
          return _DatabaseName;
        }
      }
      set {
        _DatabaseName = value ?? "";
      }
    }

    /// <summary>
    /// Current user name (SQL identification)
    /// </summary>
    public virtual string UserName {
      get {

        return _UserName ?? "";
      }
      set {
        _UserName = value ?? "";
      }
    }

    /// <summary>
    /// Current user password (SQL identification)
    /// </summary>
    public virtual string Password {
      get {
        return _Password ?? "";

      }
      set {
        _Password = value ?? "";
      }
    }

    public virtual bool UseIntegratedSecurity {
      get {
        return _UseIntegratedSecurity;

      }
      set {
        _UseIntegratedSecurity = value;
      }
    }

    /// <summary>
    /// Obtain the connection string based on current properties of the object
    /// </summary>
    public virtual string ConnectionString {
      get {
        SqlConnectionStringBuilder ConnectionBuilder = new SqlConnectionStringBuilder();
        ConnectionBuilder.DataSource = ServerName;
        ConnectionBuilder.InitialCatalog = DatabaseName;
        ConnectionBuilder.ConnectTimeout = ConnectionTimeout;
        ConnectionBuilder.MultipleActiveResultSets = UseMars;
        ConnectionBuilder.Pooling = UsePooledConnections;
        ConnectionBuilder.IntegratedSecurity = UseIntegratedSecurity;
        ConnectionBuilder.UserID = UserName;
        ConnectionBuilder.Password = Password;
        return ConnectionBuilder.ToString();
      }
    }
    #endregion Connection string parameters

    /// <summary>
    /// The underlying Transaction
    /// </summary>
    public SqlTransaction Transaction { get; private set; }

    public SqlSchema Schema { get; set; }
    #endregion Public properties

    #region Private variables
    private string _ServerName;
    private string _DatabaseName;
    private string _UserName;
    private string _Password;
    private bool _UseIntegratedSecurity;
    private int _ConnectionTimeout;
    #endregion Private variables

    #region Constructor(s)
    /// <summary>
    /// Builds a database object based on default values
    /// </summary>
    public TSqlDatabase(bool integratedSecurity = false)
      : this(DEFAULT_SERVERNAME, DEFAULT_DATABASENAME, DEFAULT_USERNAME, DEFAULT_PASSWORD) {
      UseIntegratedSecurity = integratedSecurity;
      if (UseIntegratedSecurity) {
        UserName = "";
        Password = "";
      }
    }

    public TSqlDatabase(string connectionString) {
      InitConnection(ParseConnectionString(connectionString));
    }



    /// <summary>
    /// Builds a database object based on prodived server and database. User name and password are default values
    /// </summary>
    /// <param name="serverName">Name of the server to connect (SERVER or SERVER\INSTANCE). Use "(local)" for server on local machine</param>
    /// <param name="databaseName">Name of the database to open</param>
    public TSqlDatabase(string serverName, string databaseName, bool integratedSecurity = false)
      : this(serverName, databaseName, DEFAULT_USERNAME, DEFAULT_PASSWORD) {
      UseIntegratedSecurity = integratedSecurity;
      if (UseIntegratedSecurity) {
        UserName = "";
        Password = "";
      }
    }
    /// <summary>
    /// Builds a database object based on provided server, database, user and password.
    /// </summary>
    /// <remarks>Leaving username and password blank will use Integrated authentication. Otherwise, SQL authentication is used.</remarks>
    /// <param name="serverName">Name of the server to connect (SERVER or SERVER\INSTANCE). Use "(local)" for server on local machine</param>
    /// <param name="databaseName">Name of the database to open</param>
    /// <param name="userName">User name (blank = Windows authentication)</param>
    /// <param name="password">Paswword</param>
    public TSqlDatabase(string serverName, string databaseName, string userName, string password) {
      ServerName = serverName;
      DatabaseName = databaseName;
      UserName = userName;
      Password = password;
      UseIntegratedSecurity = false;
      ConnectionTimeout = 5;
      UsePooledConnections = DefaultUsePooledConnections;
      UseMars = DefaultUseMars;
      if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(DatabaseName)) {
        if (DebugMode) {
          Trace.WriteLine(string.Format("Missing information for database ConnectionString: {0}", ConnectionString), Severity.Warning);
        }
        //throw new ApplicationException(string.Format("Missing information for database ConnectionString: {0}", ConnectionString));
      }
      Schema = new SqlSchema();
    }

    public TSqlDatabase(TSqlDatabase sqlDatabase) {
      ConnectionTimeout = sqlDatabase.ConnectionTimeout;
      UsePooledConnections = sqlDatabase.UsePooledConnections;
      UseMars = sqlDatabase.UseMars;
      ServerName = sqlDatabase.ServerName;
      DatabaseName = sqlDatabase.DatabaseName;
      UserName = sqlDatabase.UserName;
      Password = sqlDatabase.Password;
      UseIntegratedSecurity = sqlDatabase.UseIntegratedSecurity;
      Schema = new SqlSchema(Schema);
    }
    public virtual void Dispose() {
      if (IsOpened) {
        TryClose();
      }
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      return ToString(true);
    }
    public string ToString(bool hiddenPassword) {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("Database \"{0}\" is {1}", Name, IsOpened ? "opened" : "closed");
      if (Transaction != null) {
        RetVal.Append(", Transaction is active");
      } else {
        RetVal.Append(", No transaction");
      }
      if (DebugMode) {
        if (hiddenPassword) {
          RetVal.AppendFormat(", ConnectionString = \"{0}\"", _HidePasswordFromConnectionString(ConnectionString));
        } else {
          RetVal.AppendFormat(", ConnectionString = \"{0}\"", ConnectionString);
        }
      }
      return RetVal.ToString();
    }
    #endregion Converters

    #region Public methods

    #region Opening / Closing
    /// <summary>
    /// Open the current database and catch any errors
    /// </summary>
    /// <returns>true if database is opened, false otherwise</returns>
    public virtual bool TryOpen() {
      if (ConnectionString != "") {
        if (IsOpened) {
          TryClose();
        }
        try {
          if (!_IsConnectionStringInvalid()) {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            DateTime StartWaitingForOpen = DateTime.Now;
            while (((DateTime.Now - StartWaitingForOpen) < TimeSpan.FromMilliseconds(ConnectionTimeout * 1000)) && (Connection.State != System.Data.ConnectionState.Open)) {
              Thread.Sleep(100);
              Trace.WriteLine((DateTime.Now - StartWaitingForOpen).ToString());
            }
            if (DebugMode) {
              Trace.WriteLine(string.Format("Connection to \"{0}\" is opened.", Name));
            }
          }
        } catch (Exception ex) {
          Trace.WriteLine(string.Format("Error opening database : {0} : {2}", Name, ToString(true), ex.Message));
        }
      }
      return IsOpened;
    }
    /// <summary>
    /// Close the current database if it opened and catch any errors
    /// </summary>
    /// <remarks>If a transaction is active, it first tries to do a rollback</remarks>
    public virtual void TryClose() {
      try {
        if (Transaction != null) {
          Trace.WriteLine("Warning: Attempting to close the connection while transaction still active. Transaction will rollback.");
          RollbackTransaction();
        }
        Connection.Close();
        DateTime StartWaitingForClose = DateTime.Now;
        while (((DateTime.Now - StartWaitingForClose) < TimeSpan.FromMilliseconds(ConnectionTimeout * 1000)) && (Connection.State != System.Data.ConnectionState.Closed)) {
          Thread.Sleep(100);
        }
        if (DebugMode) {
          Trace.WriteLine(string.Format("Connection to \"{0}\" is closed.", Name));
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error closing database {0} with connection string \"{1}\" : {2}", Name, _HidePasswordFromConnectionString(ConnectionString), ex.Message));
      }
    }

    public void ClearConnectionPool() {
      if (Connection != null) {
        SqlConnection.ClearPool(Connection);
      }
    }
    public void ClearAllConnectionPools() {
      SqlConnection.ClearAllPools();
    }
    #endregion Opening / Closing

    #region Transactions
    public IDbTransaction StartTransaction() {
      if (!IsOpened) {
        TryOpen();
      }
      if (IsOpened) {
        if (Transaction == null) {
          try {
            Transaction = Connection.BeginTransaction();
            if (DebugMode) {
              Trace.WriteLine("Transaction started...");
            }
            if (OnTransactionStarted != null) {
              OnTransactionStarted(this, EventArgs.Empty);
            }
            return Transaction;
          } catch (Exception ex) {
            Trace.WriteLine(string.Format("Error during creation of transaction : {0}", ex.Message));
          }
        } else {
          Trace.WriteLine("Error: Attempting to create a transaction, but one already exists.");
        }
      } else {
        Trace.WriteLine("Error: Attempting to create a transaction, but the connection is not opened.");
      }
      return null;
    }
    public void CommitTransaction() {
      if (Transaction != null) {
        try {
          Transaction.Commit();
          if (DebugMode) {
            Trace.WriteLine("Transaction commited.");
          }
          if (OnTransactionCommit != null) {
            OnTransactionCommit(this, EventArgs.Empty);
          }
        } catch (Exception ex) {
          Trace.WriteLine(string.Format("Error during commit of transaction : {0}", ex.Message));
          try {
            Transaction.Rollback();
          } catch (Exception ex2) {
            Trace.WriteLine(string.Format("Error during rollback of transaction : {0}", ex2.Message));
          }
        } finally {
          Transaction.Dispose();
          Transaction = null;
        }
      }
    }
    public void RollbackTransaction() {
      if (Transaction != null) {
        try {
          Transaction.Rollback();
          if (DebugMode) {
            Trace.WriteLine("Transaction rollbacked.");
          }
          if (OnTransactionRollback != null) {
            OnTransactionRollback(this, EventArgs.Empty);
          }
        } catch (Exception ex) {
          Trace.WriteLine(string.Format("Error during rollback of transaction : {0}", ex.Message));
        } finally {
          Transaction.Dispose();
          Transaction = null;
        }
      }
    }
    #endregion Transactions

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
        } catch (Microsoft.SqlServer.Management.Smo.FailedOperationException ex) {
          Trace.WriteLine(string.Format("Error restoring database : ({0}) {1}", ex.SmoExceptionType, ex.Message), Severity.Error);
          if (OnRestoreCompleted != null) {
            OnRestoreCompleted(this, new BoolEventArgs(false));
          }
          return false;
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

    #region Database management
    public bool Attach(string physicalDataFile, string physicalLogFile) {
      if (DebugMode) {
        Trace.WriteLine(string.Format("Attaching database \"{0}\" from physical locations : data=\"{1}\", log=\"{2}\"", DatabaseName, physicalDataFile, physicalLogFile));
      }

      if (!File.Exists(physicalDataFile) || !File.Exists(physicalLogFile)) {
        string Message = string.Format("Database \"{0}\" cannot be attached : data or log component is missing or access is denied", DatabaseName);
        Trace.WriteLine(Message, Severity.Error);
        if (OnDatabaseAttached != null) {
          OnDatabaseAttached(this, new BoolAndMessageEventArgs(false, Message));
        }
        return false;
      }

      StringCollection DBToAttach = new StringCollection();
      DBToAttach.Add(physicalDataFile);
      DBToAttach.Add(physicalLogFile);

      TSqlServer CurrentServer;
      try {
        CurrentServer = new TSqlServer(ServerName, UserName, Password);
        CurrentServer.AttachDatabase(DatabaseName, DBToAttach);
        if (DebugMode) {
          Trace.WriteLine("Attach is successfull.");
        }
        if (OnDatabaseAttached != null) {
          OnDatabaseAttached(this, new BoolAndMessageEventArgs(true));
        }
        return true;
      } catch (Microsoft.SqlServer.Management.Smo.FailedOperationException ex) {
        Trace.WriteLine(string.Format("Unable to attach database \"{0}\" : ({1}) {2}", DatabaseName, ex.SmoExceptionType, ex.Message), Severity.Error);
        Trace.Indent();
        Trace.WriteLine(string.Format("Inner exception : {0}", ex.InnerException.Message), Severity.Error);
        Trace.Unindent();
        if (OnDatabaseAttached != null) {
          OnDatabaseAttached(this, new BoolAndMessageEventArgs(false, ex.Message));
        }
        return false;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Generic error attaching database : {0}", ex.Message), Severity.Error);
        Trace.Indent();
        Trace.WriteLine(string.Format("Inner exception : {0}", ex.InnerException.Message), Severity.Error);
        Trace.Unindent();
        if (OnDatabaseAttached != null) {
          OnDatabaseAttached(this, new BoolAndMessageEventArgs(false, ex.Message));
        }
        return false;
      } finally {
        CurrentServer = null;
      }
    }
    public bool Exists() {
      try {
        using (TSqlServer CurrentServer = new TSqlServer(ServerName, UserName, Password)) {
          foreach (Database DatabaseItem in CurrentServer.SmoServer.Databases) {
            if (DatabaseItem.Name.ToLower() == DatabaseName.ToLower()) {
              Trace.WriteLine(string.Format("Found database \"{0}\".", DatabaseName));
              if (OnDatabaseExists != null) {
                OnDatabaseExists(this, new BoolAndMessageEventArgs(true));
              }
              return true;
            }
          }
        }
        return false;
      } catch (Microsoft.SqlServer.Management.Smo.FailedOperationException ex) {
        Trace.WriteLine(string.Format("Unable to verify the existence of database \"{0}\" : ({1}) {2}", DatabaseName, ex.SmoExceptionType, ex.Message), Severity.Error);
        if (OnDatabaseExists != null) {
          OnDatabaseExists(this, new BoolAndMessageEventArgs(false, ex.Message));
        }
        return false;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Generic error testing for existence of database : {0}", ex.Message), Severity.Error);
        Trace.WriteLine(string.Format("  Inner exception : {0}", ex.InnerException.Message), Severity.Error);
        if (OnDatabaseExists != null) {
          OnDatabaseExists(this, new BoolAndMessageEventArgs(false, ex.Message));
        }
        return false;
      }
    }
    public bool Drop(bool killConnections = true) {
      try {
        using (TSqlServer CurrentSqlServer = new TSqlServer(ServerName, UserName, Password)) {
          if (killConnections) {
            Trace.WriteLine(string.Format("Killing connection and dropping database \"{0}\"", DatabaseName));
            CurrentSqlServer.SmoServer.KillDatabase(DatabaseName);
          } else {
            Trace.WriteLine(string.Format("Dropping database \"{0}\"", DatabaseName));
            Database CurrentDb = CurrentSqlServer.SmoServer.Databases[DatabaseName];
            CurrentDb.Drop();
          }
        }
        if (OnDatabaseDropped != null) {
          OnDatabaseDropped(this, new BoolAndMessageEventArgs(true));
        }
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to drop database : {0}", ex.Message));
        if (ex.InnerException != null) {
          Trace.WriteLine(string.Format("  {0}", ex.InnerException.Message));
        }
        if (OnDatabaseDropped != null) {
          OnDatabaseDropped(this, new BoolAndMessageEventArgs(false, ex.Message));
        }
        return false;
      }
    }
    public bool Create() {
      try {
        using (TSqlServer CurrentSqlServer = new TSqlServer(ServerName, UserName, Password)) {
          Database NewDb = new Database(CurrentSqlServer.SmoServer, DatabaseName);
          Trace.WriteLine(string.Format("Creation of database \"{0}\"", DatabaseName));
          NewDb.Create();
        }
        if (OnDatabaseCreated != null) {
          OnDatabaseCreated(this, new BoolAndMessageEventArgs(true));
        }
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to create database : {0}", ex.Message));
        if (ex.InnerException != null) {
          Trace.WriteLine(string.Format("  {0}", ex.InnerException.Message));
        }
        if (OnDatabaseCreated != null) {
          OnDatabaseCreated(this, new BoolAndMessageEventArgs(false, ex.Message));
        }
        return false;
      }
    }
    #endregion Database management

    #region Schema
    public bool ReadSchema() {
      return ReadSchema(Schema.Location);
    }
    public bool ReadSchema(string schemaUri) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(schemaUri)) {
        Trace.WriteLine(string.Format("Schema for database \"{0}\" cannot be read : schema URI is null or empty", DatabaseName), Severity.Error);
        return false;
      }
      if (!File.Exists(schemaUri)) {
        Trace.WriteLine(string.Format("Schema for database \"{0}\" cannot be read : schema URI \"{1}\" is invalid or access is denied", DatabaseName, schemaUri), Severity.Error);
        return false;
      }
      #endregion Validate parameters
      Schema = new SqlSchema(DatabaseName, schemaUri);
      try {
        Schema.ReadFromXml();
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to read schema \"{0}\" for database \"{1}\" : {2}", schemaUri, DatabaseName, ex.Message));
        return false;
      }
    }
    public bool ReadSchema(Stream schemaStream) {
      #region Validate parameters
      if (schemaStream == null) {
        Trace.WriteLine(string.Format("Schema for database \"{0}\" cannot be read : schema stream is null", DatabaseName), Severity.Error);
        return false;
      }
      #endregion Validate parameters
      Schema = new SqlSchema(DatabaseName, "");
      try {
        Schema.ReadFromXml(schemaStream);
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to read schema for database \"{0}\" : {1}", DatabaseName, ex.Message));
        return false;
      }
    }

    public bool CreateSchema() {
      #region Validate parameters
      if (Schema == null) {
        Trace.WriteLine(string.Format("Unable to create schema for database \"{0}\" : schema is null", DatabaseName), Severity.Error);
        return false;
      }
      #endregion Validate parameters
      using (TSqlServer CurrentServer = new TSqlServer(ServerName, UserName, Password)) {
        try {
          Database CurrentDb = CurrentServer.SmoServer.Databases[DatabaseName];
          Schema.Create(CurrentDb);
          return true;
        } catch (Exception ex) {
          Trace.WriteLine(string.Format("Unable to create schema for database \"{0}\" : {1}", DatabaseName, ex.Message));
          return false;
        }
      }
    }
    #endregion Schema

    #region Tables management
    public virtual bool TableExists(string table) {
      try {
        using (TSqlServer CurrentServer = new TSqlServer(ServerName, UserName, Password)) {
          Database CurrentDatabase = CurrentServer.SmoServer.Databases[DatabaseName];
          TableCollection Tables = CurrentDatabase.Tables;
          return (Tables.Contains(table));
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to test existence of table {0} in database {1} : {2}", table, DatabaseName, ex.Message), Severity.Error);
        Trace.WriteLine(string.Format("  Inner exception : {0}", ex.InnerException.Message), Severity.Error);
        return false;
      }
    }

    public virtual IEnumerable<string> ListTables() {
      List<string> RetVal = new List<string>();
      try {
        using (TSqlServer CurrentServer = new TSqlServer(ServerName, UserName, Password)) {
          Database CurrentDatabase = CurrentServer.SmoServer.Databases[DatabaseName];
          foreach (Table TableItem in CurrentDatabase.Tables) {
            RetVal.Add(TableItem.Name);
          }
          return RetVal;
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to obtain the list of tables from database {0} : {1}", DatabaseName, ex.Message), Severity.Error);
        Trace.WriteLine(string.Format("  Inner exception : {0}", ex.InnerException.Message), Severity.Error);
        return null;
      }
    }
    public virtual void CreateTables() {
      throw new NotImplementedException();
    }
    public virtual void DropTables() {
      StringBuilder Message = new StringBuilder();
      Message.AppendFormat("Dropping all tables from database \"{0}\"\n", DatabaseName);
      try {
        using (TSqlServer CurrentServer = new TSqlServer(ServerName, UserName, Password)) {
          Database CurrentDatabase = CurrentServer.SmoServer.Databases[DatabaseName];
          List<Table> Tables = new List<Table>();
          foreach (Table TableItem in CurrentDatabase.Tables) {
            Tables.Add(TableItem);
          }
          foreach (Table TableItem in Tables) {
            Message.AppendFormat("  Dropping table \"{0}\"...", TableItem.Name);
            TableItem.Drop();
            Message.AppendFormat(" OK\n");
          }
        }
        Message.Append("Done.");
      } catch (Exception ex) {
        Message.Append(" FAILED\n");
        Message.AppendFormat("  Unable to drop tables from database {0} : {1}\n", DatabaseName, ex.Message);
        Message.AppendFormat("    Inner exception : {0}\n", ex.InnerException.Message);
      } finally {
        Trace.WriteLine(Message.ToString());
      }
    }
    public virtual void DropTable(string table) {
      StringBuilder Message = new StringBuilder();
      Message.AppendFormat("Dropping table \"{0}\" from database \"{1}\"...", table, DatabaseName);
      try {
        using (TSqlServer CurrentServer = new TSqlServer(ServerName, UserName, Password)) {
          Database CurrentDatabase = CurrentServer.SmoServer.Databases[DatabaseName];
          foreach (Table TableItem in CurrentDatabase.Tables) {
            if (TableItem.Name == table) {
              TableItem.Drop();
              Message.AppendFormat(" OK\n");
              if (OnTableDropped != null) {
                OnTableDropped(this, new BoolAndMessageEventArgs(true, table));
              }
              return;
            }
          }
        }
      } catch (Exception ex) {
        Message.Append(" FAILED\n");
        Message.AppendFormat("Unable to drop table {0} from database {1} : {2}", table, DatabaseName, ex.Message);
        Message.AppendFormat("  Inner exception : {0}", ex.InnerException.Message);
        if (OnTableDropped != null) {
          OnTableDropped(this, new BoolAndMessageEventArgs(false, table));
        }
      } finally {
        Trace.WriteLine(Message.ToString());
      }
    }
    #endregion Tables management

    #region Records management
    public virtual IEnumerable<T> SelectQuery<T>(string query, Func<IDataReader, T> mapMethod) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(query)) {
        Trace.WriteLine("Unable to execute a Select with a null or empty query string");
        return new List<T>();
      }
      #endregion Validate parameters
      return SelectQuery<T>(new SqlCommand(query), mapMethod);
    }
    public virtual IEnumerable<T> SelectQuery<T>(IDbCommand command, Func<IDataReader, T> mapMethod) {
      #region Validate parameters
      if (command == null) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        return new List<T>();
      }
      #endregion Validate parameters
      List<T> RetVal = new List<T>();
      bool LocalTransaction = false;
      try {
        if (!IsOpened) {
          LocalTransaction = true;
          TryOpen();
        }
        command.Connection = Connection;
        command.Transaction = Transaction;
        using (IDataReader R = command.ExecuteReader()) {
          while (R.Read()) {
            RetVal.Add(mapMethod(R));
          }
          R.Close();
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error during select : {0} : {1}", command.CommandText, ex.Message));
      } finally {
        if (LocalTransaction) {
          TryClose();
        }
      }
      return RetVal;
    }
    public virtual IEnumerable<T> SelectQuery<T>(IDbCommand command, Func<TRecordCacheCollection, T> mapMethod) {

      List<T> RetVal = new List<T>();
      bool LocalTransaction = false;
      try {
        if (!IsOpened) {
          LocalTransaction = true;
          TryOpen();
        }
        command.Connection = Connection;
        command.Transaction = Transaction;

        TRecordCacheCollection Records;

        using (IDataReader R = command.ExecuteReader()) {
          Records = new TRecordCacheCollection(R);
          R.Close();
        }
        while (Records.Read() != null) {
          RetVal.Add(mapMethod(Records));
        }

      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error during select : {0} : {1}", command.ToString(), ex.Message));
      } finally {
        if (LocalTransaction) {
          TryClose();
        }
      }
      return RetVal;
    }

    public virtual T SelectQueryRecord<T>(string query, Func<IDataReader, T> mapMethod) where T : new() {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(query)) {
        Trace.WriteLine("Unable to execute a Select with a null or empty query string");
        return default(T);
      }
      #endregion Validate parameters
      return SelectQueryRecord<T>(new SqlCommand(query), mapMethod);
    }
    public virtual T SelectQueryRecord<T>(IDbCommand command, Func<IDataReader, T> mapMethod) where T : new() {
      #region Validate parameters
      if (command == null) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        return default(T);
      }
      #endregion Validate parameters
      T RetVal = new T();
      bool LocalTransaction = false;
      try {
        if (!IsOpened) {
          TryOpen();
          LocalTransaction = true;
        }
        command.Connection = Connection;
        command.Transaction = Transaction;
        using (IDataReader R = command.ExecuteReader()) {
          R.Read();
          RetVal = mapMethod(R);
          R.Close();
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error during select : {0} : {1}", command.CommandText, ex.Message));
      } finally {
        if (LocalTransaction) {
          TryClose();
        }
      }
      return RetVal;
    }

    public virtual T SelectQueryValue<T>(string query, Func<IDataReader, T> mapMethod) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(query)) {
        Trace.WriteLine("Unable to execute a Select with a null or empty query string");
        return default(T);
      }
      #endregion Validate parameters
      return SelectQueryValue<T>(new SqlCommand(query), mapMethod);
    }
    public virtual T SelectQueryValue<T>(IDbCommand command, Func<IDataReader, T> mapMethod) {
      #region Validate parameters
      if (command == null) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        return default(T);
      }
      #endregion Validate parameters
      T RetVal = default(T);
      bool LocalTransaction = false;
      try {
        if (!IsOpened) {
          TryOpen();
          LocalTransaction = true;
        }
        command.Connection = Connection;
        command.Transaction = Transaction;
        using (IDataReader R = command.ExecuteReader()) {
          R.Read();
          RetVal = mapMethod(R);
          R.Close();
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error during select : {0} : {1}", command.CommandText, ex.Message));
      } finally {
        if (LocalTransaction) {
          TryClose();
        }
      }
      return RetVal;
    }

    public virtual bool ExecuteNonQuery(IDbTransaction transaction, params IDbCommand[] sqlCommands) {
      return ExecuteNonQuery(transaction, sqlCommands);
    }
    public virtual bool ExecuteNonQuery(IDbTransaction transaction, IEnumerable<IDbCommand> sqlCommands) {

      StringBuilder Status = new StringBuilder("Execute non query commands : ");

      try {
        Trace.Indent();

        #region Validate parameters
        if (sqlCommands == null) {
          Trace.WriteLine("ExecuteNonQuery : Unable to execute command from a null SqlCommands");
          throw new ArgumentNullException("sqlCommands");
        }
        if (sqlCommands.Count() == 0) {
          Trace.WriteLine("ExecuteNonQuery : Unable to execute command from an empty query list");
          throw new ArgumentOutOfRangeException("sqlCommands");
        }
        if (transaction == null) {
          Trace.WriteLine("ExecuteNonQuery : Unable to use a null transaction");
          throw new ArgumentNullException("transaction");
        }
        #endregion Validate parameters

        Status.AppendLine(string.Format("{0} command(s) => ", sqlCommands.Count()));

        try {

          foreach (IDbCommand SqlCommandItem in sqlCommands) {
            SqlCommandItem.Connection = Connection;
            SqlCommandItem.Transaction = transaction;
            SqlCommandItem.ExecuteNonQuery();
          }

          Status.Append("successfull");
          transaction.Commit();

        } catch (Exception ex) {
          Status.AppendFormat("failed : {0}", ex.Message);
          transaction.Rollback();
          return false;
        }

        return true;
      } finally {
        Trace.Unindent();
        Trace.WriteLine(Status.ToString());
      }
    }
    public virtual bool ExecuteNonQuery(params IDbCommand[] sqlCommands) {
      return ExecuteNonQuery(sqlCommands);
    }
    public virtual bool ExecuteNonQuery(IEnumerable<IDbCommand> sqlCommands) {
      StringBuilder Status = new StringBuilder("Execute non query commands : ");

      try {
        Trace.Indent();

        #region Validate parameters
        if (sqlCommands == null) {
          Trace.WriteLine("Unable to create a record from a null SqlCommand");
          return false;
        }
        if (sqlCommands.Count() == 0) {
          Trace.WriteLine("Unable to create a record from an empty query list");
          return false;
        }
        #endregion Validate parameters

        Status.AppendFormat("{0} command(s)\r\n=> ", sqlCommands.Count());

        try {

          TryOpen();
          StartTransaction();

          foreach (IDbCommand SqlCommandItem in sqlCommands) {
            SqlCommandItem.Connection = Connection;
            SqlCommandItem.Transaction = Transaction;
            if (SqlCommandItem.ExecuteNonQuery() == 0) {
              RollbackTransaction();
              Status.AppendFormat("failed : {0}", SqlCommandItem.CommandText);
              return false;
            }
          }

          CommitTransaction();
          Status.Append("successfull");

        } catch (Exception ex) {
          Status.AppendFormat("failed : {0}", ex.Message);
          RollbackTransaction();
          return false;

        } finally {
          TryClose();
        }
        return true;
      } finally {
        Trace.Unindent();
        Trace.WriteLine(Status.ToString());
      }
    }
    //public virtual T ExecuteNonQuery<T>(params SqlCommand[] sqlCommands) where T : new() {
    //  StringBuilder Status = new StringBuilder("Execute non query commands : ");

    //  try {
    //    Trace.Indent();

    //    #region Validate parameters
    //    if (sqlCommands == null) {
    //      Trace.WriteLine("Unable to create a record from a null SqlCommand");
    //      return default(T);
    //    }
    //    if (sqlCommands.Length == 0) {
    //      Trace.WriteLine("Unable to create a record from an empty query list");
    //      return default(T);
    //    }
    //    #endregion Validate parameters

    //    Status.AppendFormat("{0} command(s)\r\n=> ", sqlCommands.Length);

    //    try {

    //      TryOpen();
    //      StartTransaction();

    //      foreach (SqlCommand SqlCommandItem in sqlCommands) {
    //        SqlCommandItem.Connection = Connection;
    //        SqlCommandItem.Transaction = Transaction;
    //        if (SqlCommandItem.ExecuteScalar() == null) {
    //          RollbackTransaction();
    //          Status.AppendFormat("failed : {0}", SqlCommandItem.CommandText);
    //          return default(T);
    //        }
    //      }

    //      CommitTransaction();
    //      Status.Append("successfull");

    //    } catch (Exception ex) {
    //      Status.AppendFormat("failed : {0}", ex.Message);
    //      RollbackTransaction();
    //      return default(T);

    //    } finally {
    //      TryClose();
    //    }
    //    return ;
    //  } finally {
    //    Trace.Unindent();
    //    Trace.WriteLine(Status.ToString());
    //  }
    //}
    #endregion Records management

    #endregion Public methods

    #region Private methods
    private Dictionary<string, string> ParseConnectionString(string connectionString) {
      Dictionary<string, string> RetVal = new Dictionary<string, string>();
      string[] ConnectionStringComponents = connectionString.Split(';');
      foreach (string ComponentItem in ConnectionStringComponents) {
        try {
          string[] ComponentKeyValuePair = ComponentItem.Split('=');
          RetVal.Add(ComponentKeyValuePair[0].ToLower(), ComponentKeyValuePair[1]);
        } catch (Exception ex) {
          Trace.WriteLine(string.Format("Error while parsing connection string \"{0}\" : {1}", connectionString, ex.Message));
        }
      }

      AddDefaultKeyValue(RetVal, CS_SERVERNAME, DEFAULT_SERVERNAME);
      AddDefaultKeyValue(RetVal, CS_DATABASENAME, DEFAULT_DATABASENAME);
      AddDefaultKeyValue(RetVal, CS_USERID, DEFAULT_USERNAME);
      AddDefaultKeyValue(RetVal, CS_USERNAME, DEFAULT_USERNAME);
      AddDefaultKeyValue(RetVal, CS_PASSWORD, DEFAULT_PASSWORD);
      AddDefaultKeyValue(RetVal, CS_USE_MARS, "true");

      return RetVal;
    }

    private void AddDefaultKeyValue(Dictionary<string, string> dict, string key, string defaultValue) {
      if (!dict.ContainsKey(key)) {
        dict.Add(key, defaultValue);
      }
    }

    private void InitConnection(Dictionary<string, string> parsedConnectionString) {
      ServerName = parsedConnectionString[CS_SERVERNAME];
      DatabaseName = parsedConnectionString[CS_DATABASENAME];
      UserName = parsedConnectionString[CS_USERID] != "" ? parsedConnectionString[CS_USERID] : parsedConnectionString[CS_USERNAME];
      Password = parsedConnectionString[CS_PASSWORD];
      UseMars = parsedConnectionString[CS_USE_MARS].ToBool();
      if (_IsConnectionStringInvalid()) {
        if (DebugMode) {
          Trace.Write(string.Format("Missing information for database ConnectionString: {0}", ConnectionString), Severity.Warning);
        }
      }
    }
    private bool _IsConnectionStringInvalid() {
      if (ServerName == "" || DatabaseName == "") {
        return true;
      } else {
        return false;
      }
    }
    private string _HidePasswordFromConnectionString(string connectionString) {
      string[] ConnectionStringItems = connectionString.Split(';');
      for (int i = 0; i < ConnectionStringItems.Length; i++) {
        if (ConnectionStringItems[i].StartsWith("Password=")) {
          ConnectionStringItems[i] = "Password=xxx(hidden)xxx";
        }
      }
      return string.Join(";", ConnectionStringItems);
    }
    #endregion Private methods

    #region Events
    public event EventHandler<BoolEventArgs> OnBackupCompleted;
    public event EventHandler<BoolEventArgs> OnRestoreCompleted;

    public event EventHandler<BoolAndMessageEventArgs> OnDatabaseAttached;
    public event EventHandler<BoolAndMessageEventArgs> OnDatabaseExists;
    public event EventHandler<BoolAndMessageEventArgs> OnDatabaseCreated;
    public event EventHandler<BoolAndMessageEventArgs> OnDatabaseDropped;

    public event EventHandler OnTransactionStarted;
    public event EventHandler OnTransactionCommit;
    public event EventHandler OnTransactionRollback;

    public event EventHandler<BoolAndMessageEventArgs> OnTableDropped;
    #endregion Events

  }

}
