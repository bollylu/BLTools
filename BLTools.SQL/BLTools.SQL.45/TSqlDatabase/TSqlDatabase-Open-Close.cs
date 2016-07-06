using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLTools.SQL {
  public partial class TSqlDatabase {

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

  }
}
