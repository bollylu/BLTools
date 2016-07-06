using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.SQL {
  public partial class TSqlDatabase {

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


  }
}
