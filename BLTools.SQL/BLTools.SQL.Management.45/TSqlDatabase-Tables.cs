using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.SQL.Management {
  public partial class TSqlDatabaseManager : TSqlDatabase {

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

  }
}
