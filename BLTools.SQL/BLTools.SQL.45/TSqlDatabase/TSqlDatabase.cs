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
using System.Threading.Tasks;

namespace BLTools.SQL {
  public partial class TSqlDatabase : IDisposable, IDatabase {

    /// <summary>
    /// When set to true, provide additional debug information
    /// </summary>
#if DEBUG
    public static bool DebugMode = true;
#else
    public static bool DebugMode = false;
#endif

    #region Public properties
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
    /// The underlying Transaction
    /// </summary>
    public SqlTransaction Transaction { get; private set; }

    #endregion Public properties

    

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

    #endregion Public methods

    

  }

}
