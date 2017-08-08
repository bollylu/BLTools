using BLTools.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.SQL {
  public partial class TSqlDatabase {

    #region Records management
    public virtual async Task<IEnumerable<T>> SelectQueryAsync<T>(string query, Func<IDataReader, T> mapMethod) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(query)) {
        Trace.WriteLine("Unable to execute a Select with a null or empty query string");
        return new List<T>();
      }
      #endregion Validate parameters
      return await SelectQueryAsync<T>(new SqlCommand(query), mapMethod);
    }
    public virtual async Task<IEnumerable<T>> SelectQueryAsync<T>(SqlCommand command, Func<IDataReader, T> mapMethod) {
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
        using (IDataReader R = await command.ExecuteReaderAsync()) {
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
    public virtual async Task<IEnumerable<T>> SelectQueryAsync<T>(SqlCommand command, Func<TRecordCacheCollection, T> mapMethod) {

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

        using (IDataReader R = await command.ExecuteReaderAsync()) {
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

    public virtual async Task<T> SelectQueryRecordAsync<T>(string query, Func<IDataReader, T> mapMethod) where T : new() {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(query)) {
        Trace.WriteLine("Unable to execute a Select with a null or empty query string");
        return default(T);
      }
      #endregion Validate parameters
      return await SelectQueryRecordAsync<T>(new SqlCommand(query), mapMethod);
    }
    public virtual async Task<T> SelectQueryRecordAsync<T>(SqlCommand command, Func<IDataReader, T> mapMethod) where T : new() {
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
        using (IDataReader R = await command.ExecuteReaderAsync()) {
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

    public virtual async Task<T> SelectQueryValueAsync<T>(string query, Func<IDataReader, T> mapMethod) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(query)) {
        Trace.WriteLine("Unable to execute a Select with a null or empty query string");
        return default(T);
      }
      #endregion Validate parameters
      return await SelectQueryValueAsync<T>(new SqlCommand(query), mapMethod);
    }
    public virtual async Task<T> SelectQueryValueAsync<T>(SqlCommand command, Func<IDataReader, T> mapMethod) {
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
        using (IDataReader R = await command.ExecuteReaderAsync()) {
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

    public virtual async Task<bool> ExecuteNonQueryAsync(SqlTransaction transaction, params SqlCommand[] sqlCommands) {
      return await ExecuteNonQueryAsync(transaction, sqlCommands);
    }
    public virtual async Task<bool> ExecuteNonQueryAsync(SqlTransaction transaction, IEnumerable<SqlCommand> sqlCommands) {

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

          foreach (SqlCommand SqlCommandItem in sqlCommands) {
            SqlCommandItem.Connection = Connection;
            SqlCommandItem.Transaction = transaction;
            await SqlCommandItem.ExecuteNonQueryAsync();
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
    public virtual async Task<bool> ExecuteNonQueryAsync(params SqlCommand[] sqlCommands) {
      return await ExecuteNonQueryAsync(sqlCommands);
    }
    public virtual async Task<bool> ExecuteNonQueryAsync(IEnumerable<SqlCommand> sqlCommands) {
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

          foreach (SqlCommand SqlCommandItem in sqlCommands) {
            SqlCommandItem.Connection = Connection;
            SqlCommandItem.Transaction = Transaction;
            if (await SqlCommandItem.ExecuteNonQueryAsync() == 0) {
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

  }
}
