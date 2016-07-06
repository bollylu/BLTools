using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace BLTools.SQL.Management {
  public class TSqlServer : IDisposable {

    #region Default values
    /// <summary>
    /// This is the default server name (including instance)
    /// </summary>
    public static string DefaultServerName = "(local)";

    /// <summary>
    /// This is the default username. Blank means we will use OS username and password.
    /// </summary>
    public static string DefaultUserName = "";

    /// <summary>
    /// This the default password. Only used if Username is provided.
    /// </summary>
    public static string DefaultPassword = "";

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
#if DEBUG
    public static bool DebugMode = true;
#else
    public static bool DebugMode = false;
#endif

    /// <summary>
    /// Name of the server
    /// </summary>
    public string Name {
      get {
        return string.Format("{0}", ServerName ?? "");
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
    /// The underlying connection to the server
    /// </summary>
    public SqlConnection Connection { get; private set; }

    /// <summary>
    /// The underlying Transaction
    /// </summary>
    public SqlTransaction Transaction { get; private set; }

    /// <summary>
    /// Returns a new SMO connection based on the current server and database parameters
    /// </summary>
    public ServerConnection SmoConnection {
      get {
        return new ServerConnection(new SqlConnection(ConnectionString));
      }
    }

    /// <summary>
    /// Returns an instance of the SmoServer based on the SmoConnection. The instance is cached.
    /// </summary>
    public Server SmoServer {
      get {
        if (_SmoServer == null) {
          _SmoServer = new Server(SmoConnection);
        }
        return _SmoServer;
      }
      private set {
        _SmoServer = value;
      }
    }
    private Server _SmoServer;

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
    public bool UsePooledConnections { get; set; }

    /// <summary>
    /// True to use MARS. False otherwise.
    /// </summary>
    /// <remarks>Only available with SQL 2005 and up.</remarks>
    public bool UseMars { get; set; }

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

    /// <summary>
    /// Obtain the connection string based on current properties of the object
    /// </summary>
    public string ConnectionString {
      get {
        SqlConnectionStringBuilder ConnectionBuilder = new SqlConnectionStringBuilder();
        ConnectionBuilder.DataSource = ServerName;
        ConnectionBuilder.InitialCatalog = "";
        ConnectionBuilder.ConnectTimeout = ConnectionTimeout;
        ConnectionBuilder.MultipleActiveResultSets = UseMars;
        ConnectionBuilder.Pooling = UsePooledConnections;
        if (UserName == "") {
          ConnectionBuilder.IntegratedSecurity = true;
        } else {
          ConnectionBuilder.IntegratedSecurity = false;
          ConnectionBuilder.UserID = UserName;
          ConnectionBuilder.Password = Password;
        }
        return ConnectionBuilder.ToString();
      }
    }
    #endregion Connection string parameters

    /// <summary>
    /// Indicate whether the specified server exists or not
    /// </summary>
    public bool Exists { get; private set; }

    /// <summary>
    /// Returns the Sql server Version
    /// </summary>
    public Version SqlServerVersion { get; private set; }

    /// <summary>
    /// Returns the Sql server Level
    /// </summary>
    public string SqlServerLevel { get; private set; }

    /// <summary>
    /// Returns the Sql server Edition (e.g. Standard, Datacenter, ...)
    /// </summary>
    public string SqlServerEdition { get; private set; }
    #endregion Public properties

    #region Private variables
    private string _ServerName;
    private string _UserName;
    private string _Password;
    private int _ConnectionTimeout;
    #endregion Private variables

    #region Constructor(s)
    /// <summary>
    /// Builds a SqlServer object based on default values
    /// </summary>
    public TSqlServer() : this(DefaultServerName, DefaultUserName, DefaultPassword) { }

    /// <summary>
    /// Builds a SqlServer object based on prodived server. User name and password are default values
    /// </summary>
    /// <param name="serverName">Name of the server to connect (SERVER or SERVER\INSTANCE). Use "(local)" for server on local machine</param>
    public TSqlServer(string serverName) : this(serverName, DefaultUserName, DefaultPassword) { }

    /// <summary>
    /// Builds a SqlServer object based on provided server, user and password.
    /// </summary>
    /// <remarks>Leaving username and password blank will use Integrated authentication. Otherwise, SQL authentication is used.</remarks>
    /// <param name="serverName">Name of the server to connect (SERVER or SERVER\INSTANCE). Use "(local)" for server on local machine</param>
    /// <param name="userName">User name (blank = Windows authentication)</param>
    /// <param name="password">Paswword</param>
    public TSqlServer(string serverName, string userName, string password) {
      ServerName = serverName;
      UserName = userName;
      Password = password;
      ConnectionTimeout = 5;
      UsePooledConnections = DefaultUsePooledConnections;
      UseMars = DefaultUseMars;
      SqlServerVersion = null;
      SqlServerLevel = null;
      SqlServerEdition = null;
      if (string.IsNullOrEmpty(ServerName)) {
        if (DebugMode) {
          Trace.WriteLine(string.Format("Missing information for SqlServer ConnectionString: {0}", ConnectionString), Severity.Warning);
        }
        Exists = false;
      } else {
        try {
          SqlServerVersion = SmoServer.Version;
          SqlServerLevel = SmoServer.ProductLevel;
          SqlServerEdition = SmoServer.Edition;
          Exists = true;
        } catch {
          Exists = false;
        }
      }
    }
    public TSqlServer(TSqlServer sqlServer) {
      ServerName = sqlServer.ServerName;
      UserName = sqlServer.UserName;
      Password = sqlServer.Password;
      ConnectionTimeout = sqlServer.ConnectionTimeout;
      UsePooledConnections = sqlServer.UsePooledConnections;
      UseMars = sqlServer.UseMars;
      SqlServerVersion = null;
      SqlServerLevel = null;
      SqlServerEdition = null;
      if (string.IsNullOrEmpty(ServerName)) {
        Trace.WriteLine(string.Format("Missing information for SqlServer ConnectionString: {0}", ConnectionString), Severity.Warning);
        Exists = false;
      } else {
        try {
          SqlServerVersion = SmoServer.Version;
          SqlServerLevel = SmoServer.ProductLevel;
          SqlServerEdition = SmoServer.Edition;
          Exists = true;
        } catch {
          Exists = false;
        }
      }
    }
    /// <summary>
    /// Close any opened connection through the SMO
    /// </summary>
    public virtual void Dispose() {
      SmoServer = null;
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("SqlServer \"{0}\" is {1}", Name, IsOpened ? "opened" : "closed");
      if (Transaction != null) {
        RetVal.AppendFormat(", Transaction is active");
      }
      RetVal.AppendFormat(", ConnectionString = \"{0}\"", _HidePasswordFromConnectionString(ConnectionString));
      return RetVal.ToString();
    }
    #endregion Converters

    #region Public methods
    /// <summary>
    /// Attach a database to the current server
    /// </summary>
    /// <param name="databaseName">The name of the attached database</param>
    /// <param name="filesToAttach">The list of physical files to attach</param>
    public void AttachDatabase(string databaseName, StringCollection filesToAttach) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(databaseName)) {
        Trace.WriteLine("Error : unable to attach an database without name");
        return;
      }
      if (filesToAttach.Count == 0) {
        Trace.WriteLine("Error : unable to attach an database without the associated files");
        return;
      }
      #endregion Validate parameters

      try {
        if (SmoServer != null) {
          SmoServer.AttachDatabase(databaseName, filesToAttach);
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while attaching database \"{0}\" to the server \"{1}\" : {2}", databaseName, Name, ex.Message));
      }
    }
    public bool DropDatabase(string databaseName) {
      try {
        using (TSqlServer CurrentSqlServer = new TSqlServer(ServerName, UserName, Password)) {
          Database DroppedDb = CurrentSqlServer.SmoServer.Databases[databaseName];
          DroppedDb.Drop();
        }
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to drop database : {0}", ex.Message));
        if (ex.InnerException != null) {
          Trace.WriteLine(string.Format("  {0}", ex.InnerException.Message));
        }
        return false;
      }
    }
    public bool CreateDatabase(string databaseName) {
      try {
        using (TSqlServer CurrentSqlServer = new TSqlServer(ServerName, UserName, Password)) {
          Database NewDb = new Database(CurrentSqlServer.SmoServer, databaseName);
          NewDb.Create();
        }
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to create database : {0}", ex.Message));
        if (ex.InnerException != null) {
          Trace.WriteLine(string.Format("  {0}", ex.InnerException.Message));
        }
        return false;
      }
    }
    #endregion Public methods

    #region Private methods
    private bool _ConnectionStringInvalid() {
      if (ServerName == "") {
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
  }
}
