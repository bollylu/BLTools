using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advantage.Data.Provider;
using System.Diagnostics;
using BLTools;
using BLTools.Data;
using System.Threading;
using System.Data;
using System.IO;

namespace BLTools.ADS {
  public class TAdsDatabase : IDisposable, IDatabase {

    #region Constants for connection string
    private const string CS_DATA_SOURCE = "datasource";
    private const string CS_INITIAL_CATALOG = "initialcatalog";
    private const string CS_USER = "user";
    private const string CS_PASSWORD = "password";
    private const string CS_SERVER_TYPE = "servertype";
    #endregion Constants for connection string

    #region Default values
    public static string DefaultDataSource = "";
    public static string DefaultInitialCatalog = "";
    public static AdsDatabaseServerTypeEnum DefaultServerType = AdsDatabaseServerTypeEnum.REMOTE;
    public static string DefaultUserName = "";
    public static string DefaultPassword = "";
    public static bool DefaultTrimStringFields = true;
    public static string DefaultLogFilename = "";
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
    /// Location of the AdsBackup.exe utility, used for backup and restore purposes
    /// </summary>
    public static string AdsBackupLocation { get; set; }

    private static string AdsBackupFullName {
      get {
        return Path.Combine(AdsBackupLocation, "adsbackup.exe");
      }
    }
    /// <summary>
    /// Name of the database
    /// </summary>
    public string Name {
      get {
        return string.Format("{0}:{1}", ServerType.ToString(), DataSource ?? "");
      }
    }
    /// <summary>
    /// Indicate whether the connection is opened or not
    /// </summary>
    public bool IsOpened {
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
    public AdsConnection Connection {
      get;
      private set;
    }

    #region Connection string parameters
    /// <summary>
    /// The timeout when trying to connect to the database (in seconds)
    /// </summary>
    /// <remarks>Max value is 1000. If 0, then 300.</remarks>
    public int ConnectionTimeout {
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
    public bool UsePooledConnections {
      get;
      set;
    }

    /// <summary>
    /// Current Database server name
    /// </summary>
    public string DataSource {
      get {
        if (string.IsNullOrEmpty(_DataSource)) {
          return DefaultDataSource;
        } else {
          return _DataSource;
        }
      }
      set {
        _DataSource = value ?? "";
      }
    }

    public string InitialCatalog {
      get {
        if (string.IsNullOrEmpty(_InitialCatalog)) {
          return DefaultInitialCatalog;
        } else {
          return _InitialCatalog;
        }
      }
      set {
        _InitialCatalog = value ?? "";
      }
    }

    /// <summary>
    /// current database name
    /// </summary>
    public AdsDatabaseServerTypeEnum ServerType {
      get {
        if (_ServerType == AdsDatabaseServerTypeEnum.Unknown) {
          return DefaultServerType;
        } else {
          return _ServerType;
        }
      }
      set {
        _ServerType = value;
      }
    }

    /// <summary>
    /// Current user name (SQL identification)
    /// </summary>
    public string UserName {
      get {
        if (string.IsNullOrEmpty(_UserName)) {
          return DefaultUserName;
        } else {
          return _UserName;
        }
      }
      set {
        _UserName = value ?? "";
      }
    }

    /// <summary>
    /// Current user password (SQL identification)
    /// </summary>
    public string Password {
      get {
        if (string.IsNullOrEmpty(_Password)) {
          return DefaultPassword;
        } else {
          return _Password;
        }
      }
      set {
        _Password = value ?? "";
      }
    }

    public bool TrimStringFields {
      get {
        return _TrimStringFields;
      }
      set {
        _TrimStringFields = value;
      }
    }

    /// <summary>
    /// Obtain the connection string based on current properties of the object
    /// </summary>
    public string ConnectionString {
      get {
        StringBuilder RetVal = new StringBuilder();
        RetVal.AppendFormat("Data Source={0};", DataSource);
        if (InitialCatalog != "") {
          RetVal.AppendFormat("InitialCatalog={0};", InitialCatalog);
        }
        RetVal.AppendFormat("ServerType={0};", ServerType);
        RetVal.AppendFormat("Pooling={0};", UsePooledConnections);
        RetVal.AppendFormat("TrimTrailingSpaces={0}", TrimStringFields);
        if (UserName != "") {
          RetVal.AppendFormat("User ID={0};", UserName);
          RetVal.AppendFormat("Password='{0}';", UserName);
        }
        return RetVal.ToString();
      }
    }
    #endregion Connection string parameters

    /// <summary>
    /// The underlying Transaction
    /// </summary>
    public AdsTransaction Transaction {
      get;
      private set;
    }

    public string LogFilename {
      set {
        if (string.IsNullOrWhiteSpace(value)) {
          _LogFilename = DefaultLogFilename;
          Log = new TLog(_LogFilename);
        } else {
          _LogFilename = value;
          Log = new TLog();
        }
      }
      get {
        return _LogFilename;
      }
    }
    #endregion Public properties

    #region Private variables
    private string _DataSource;
    private string _InitialCatalog;
    private AdsDatabaseServerTypeEnum _ServerType;
    private string _UserName;
    private string _Password;
    private int _ConnectionTimeout;
    private bool _TrimStringFields = true;
    private string _LogFilename = "";
    private TLog Log;
    #endregion Private variables

    #region Constructor(s)
    /// <summary>
    /// Builds a database object based on default values
    /// </summary>
    public TAdsDatabase()
      : this(DefaultDataSource, DefaultInitialCatalog, DefaultServerType, DefaultUserName, DefaultPassword) {
    }

    /// <summary>
    /// Builds a database object based on prodived connection string
    /// </summary>
    /// <param name="connectionString">DataSource=(path to data location);InitialCatalog=(prefered table);ServerType=(LOCAL|REMOTE);User=(username);Password=(password)</param>
    public TAdsDatabase(string connectionString = "") {
      if (connectionString != "") {
        InitConnection(ParseConnectionString(connectionString));
      }
      Log = new TLog();
    }

    /// <summary>
    /// Builds a database object based on provided data source, initial catalog, server type, user and password.
    /// </summary>
    /// <param name="dataSource"></param>
    /// <param name="initialCatalog"></param>
    /// <param name="serverType"></param>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    public TAdsDatabase(string dataSource, string initialCatalog, AdsDatabaseServerTypeEnum serverType, string userName, string password) {
      Dictionary<string, string> ConnectionComponents = new Dictionary<string, string>();
      ConnectionComponents.Add(CS_DATA_SOURCE, dataSource);
      ConnectionComponents.Add(CS_INITIAL_CATALOG, initialCatalog);
      ConnectionComponents.Add(CS_USER, userName);
      ConnectionComponents.Add(CS_PASSWORD, password);
      ConnectionComponents.Add(CS_SERVER_TYPE, serverType.ToString());
      InitConnection(ConnectionComponents);
      Log = new TLog();
    }

    public TAdsDatabase(TAdsDatabase database) {
      if (database.ConnectionString != "") {
        InitConnection(ParseConnectionString(database.ConnectionString));
      }
      Log = new TLog();
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
    #region Opening / closing
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
            Connection = new AdsConnection(ConnectionString);
            Connection.Open();
            DateTime StartWaitingForOpen = DateTime.Now;
            while (((DateTime.Now - StartWaitingForOpen) < TimeSpan.FromMilliseconds(ConnectionTimeout * 1000)) && (Connection.State != System.Data.ConnectionState.Open)) {
              Thread.Sleep(100);
              Log.Write((DateTime.Now - StartWaitingForOpen).ToString());
            }
            if (DebugMode) {
              Log.Write(string.Format("Connection to \"{0}\" is opened.", Name));
            }
          }
        } catch (Exception ex) {
          Log.Write(string.Format("Error opening database : {0} : {1} : {2}", Name, ToString(true), ex.Message));
          if (ex.InnerException != null) {
            Log.Write(ex.InnerException.Message);
          }
        }
      }
      return IsOpened;
    }
    /// <summary>
    /// Close the current database if it is opened and catch any errors
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
          Log.Write(string.Format("Connection to \"{0}\" is closed.", Name));
        }
      } catch (Exception ex) {
        Log.Write(string.Format("Error closing database {0} with connection string \"{1}\" : {2}", Name, _HidePasswordFromConnectionString(ConnectionString), ex.Message));
      }
    }

    public virtual void Attach() {
      if (OnDatabaseAttached != null) {
        OnDatabaseAttached(this, new BoolAndMessageEventArgs(true, "database attached"));
      }
    }
    #endregion Opening / closing

    #region Transactions
    public IDbTransaction StartTransaction() {
      if (IsOpened) {
        if (Transaction == null) {
          try {
            Transaction = Connection.BeginTransaction();
            if (DebugMode) {
              Log.Write("Transaction started...");
            }
            if (OnTransactionStarted != null) {
              OnTransactionStarted(this, EventArgs.Empty);
            }
            return Transaction;
          } catch (Exception ex) {
            Log.Write(string.Format("Error during creation of transaction : {0}", ex.Message));
          }
        } else {
          Log.Write("Error: Attempting to create a transaction, but one already exists.");
        }
      } else {
        Log.Write("Error: Attempting to create a transaction, but the connection is not opened.");
      }
      return null;
    }
    public void CommitTransaction() {
      if (Transaction != null) {
        try {
          Transaction.Commit();
          if (DebugMode) {
            Log.Write("Transaction commited.");
          }
          if (OnTransactionCommit != null) {
            OnTransactionCommit(this, EventArgs.Empty);
          }
        } catch (Exception ex) {
          Log.Write(string.Format("Error during commit of transaction : {0}", ex.Message));
          try {
            Transaction.Rollback();
          } catch (Exception ex2) {
            Log.Write(string.Format("Error during rollback of transaction : {0}", ex2.Message));
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
            Log.Write("Transaction rollbacked.");
          }
          if (OnTransactionRollback != null) {
            OnTransactionRollback(this, EventArgs.Empty);
          }
        } catch (Exception ex) {
          Log.Write(string.Format("Error during rollback of transaction : {0}", ex.Message));
        } finally {
          Transaction.Dispose();
          Transaction = null;
        }
      }
    }
    #endregion Transactions

    #region Database management
    public bool Exists() {
      if (DataSource != "" && Directory.Exists(DataSource)) {
        if (Directory.GetFiles(DataSource, "*.adt").Count() > 0) {
          if (OnDatabaseExists != null) {
            OnDatabaseExists(this, new BoolAndMessageEventArgs(true));
          }
          return true;
        }
      }
      if (OnDatabaseExists != null) {
        OnDatabaseExists(this, new BoolAndMessageEventArgs(false, "No data found"));
      }
      return false;
    }
    public bool Create() {
      if (OnDatabaseCreated != null) {
        OnDatabaseCreated(this, new BoolAndMessageEventArgs(true, "Database created"));
      }
      throw new NotImplementedException();
    }
    public bool Drop(bool killConnections = true) {
      if (OnDatabaseDropped != null) {
        OnDatabaseDropped(this, new BoolAndMessageEventArgs(true, "Database dropped"));
      }
      throw new NotImplementedException();
    }
    #endregion Database management

    #region Tables management
    public IEnumerable<string> ListTables() {
      string[] RetVal = new string[] { };
      if (DataSource != "" && Directory.Exists(DataSource)) {
        RetVal = Directory.GetFiles(DataSource, "*.adt");
      }
      return RetVal;
    }
    public bool TableExists(string table) {
      if (DataSource != "" && Directory.Exists(DataSource)) {
        return File.Exists(string.Format("{0}.adt", Path.Combine(DataSource, table)));
      }
      return false;
    }

    public void CreateTables() {
      throw new NotImplementedException();
    }
    public void DropTables() {
      throw new NotImplementedException();
    }
    public void DropTable(string table) {
      throw new NotImplementedException();
    }
    #endregion Tables management

    #region Backup / restore
    public bool Backup(string backupFullName) {
      return Backup(backupFullName, true);
    }
    public bool Backup(string backupFullName, bool createDestinationFolder) {
      try {
        Trace.WriteLine("Backup request started...");
        Trace.Indent();

        #region Validate parameters
        if (string.IsNullOrWhiteSpace(backupFullName)) {
          Trace.WriteLine("Unable to create database backup whit a null or empty destination");
          return false;
        }

        if (string.IsNullOrWhiteSpace(AdsBackupLocation) || !File.Exists(AdsBackupFullName)) {
          Trace.WriteLine("Unable to locate AdsBackup.exe, cannot execute backup operation");
          return false;
        }

        if (!Directory.Exists(backupFullName)) {
          Trace.WriteLine(string.Format("Target directory is missing or access is denied : {0}", backupFullName));
          if (createDestinationFolder) {
            try {
              Trace.WriteLine(string.Format("Attempt to create the target directory : {0}", backupFullName));
              Directory.CreateDirectory(backupFullName);
            } catch (Exception ex) {
              Trace.WriteLine(string.Format("Failed : {0}", ex.Message));
              return false;
            }
          }
        }
        #endregion Validate parameters

        try {
          if (OnBackupCompleted != null) {
            OnBackupCompleted(this, new BoolEventArgs(false));
          }
          return true;
        } catch (Exception ex) {
          Trace.WriteLine(string.Format("Backup failed : {0}", ex.Message));
          return false;
        } finally {
          TryClose();
        }
      } finally {
        Trace.Unindent();
        Trace.WriteLine("Backup request completed.");
      }
    }

    public void BackupAsync(string backupFullName) {
      throw new NotImplementedException();
    }

    public bool Restore(string backupFullName) {
      if (OnRestoreCompleted != null) {
        OnRestoreCompleted(this, new BoolEventArgs(true));
      }
      throw new NotImplementedException();
    }
    #endregion Backup / restore

    #region Records management
    public virtual IEnumerable<T> SelectQuery<T>(string query, Func<IDataReader, T> mapMethod) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(query)) {
        Trace.WriteLine("Unable to execute a Select with a null or empty query string");
        return new List<T>();
      }
      #endregion Validate parameters
      return SelectQuery<T>(new AdsCommand(query), mapMethod);
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
        using (IDataReader R = ((AdsCommand)command).ExecuteExtendedReader()) {
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

        using (IDataReader R = ((AdsCommand)command).ExecuteExtendedReader()) {
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
      return SelectQueryRecord<T>(new AdsCommand(query), mapMethod);
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
        using (IDataReader R = ((AdsCommand)command).ExecuteExtendedReader()) {
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
      return SelectQueryValue<T>(new AdsCommand(query), mapMethod);
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

      AddDefaultKeyValue(RetVal, CS_DATA_SOURCE, DefaultDataSource);
      AddDefaultKeyValue(RetVal, CS_INITIAL_CATALOG, DefaultInitialCatalog);
      AddDefaultKeyValue(RetVal, CS_USER, DefaultUserName);
      AddDefaultKeyValue(RetVal, CS_PASSWORD, DefaultPassword);
      AddDefaultKeyValue(RetVal, CS_SERVER_TYPE, DefaultServerType.ToString());

      return RetVal;
    }

    private void AddDefaultKeyValue(Dictionary<string, string> dict, string key, string defaultValue) {
      if (!dict.ContainsKey(key)) {
        dict.Add(key, defaultValue);
      }
    }

    private void InitConnection(Dictionary<string, string> parsedConnectionString) {
      DataSource = parsedConnectionString[CS_DATA_SOURCE];
      InitialCatalog = parsedConnectionString[CS_INITIAL_CATALOG];
      UserName = parsedConnectionString[CS_USER];
      Password = parsedConnectionString[CS_PASSWORD];
      ServerType = (AdsDatabaseServerTypeEnum)Enum.Parse(typeof(AdsDatabaseServerTypeEnum), parsedConnectionString[CS_SERVER_TYPE]);
      if (_IsConnectionStringInvalid()) {
        if (DebugMode) {
          Log = new TLog();
          Log.Write(string.Format("Missing information for database ConnectionString: {0}", ConnectionString), ErrorLevel.Warning);
        }
      }
    }

    private bool _IsConnectionStringInvalid() {
      if (DataSource == "") {
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

    private bool ExecuteAdsBackup() {
      Process AdsBackupProcess = new Process();
      AdsBackupProcess.StartInfo = new ProcessStartInfo(AdsBackupFullName);
      AdsBackupProcess.Start();
      AdsBackupProcess.WaitForExit();

      return (AdsBackupProcess.ExitCode == 0);
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
    #endregion Events

  }
}
