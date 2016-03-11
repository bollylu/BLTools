using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLTools;
using BLTools.Data;
using BLTools.SQL;
using System.Diagnostics;
using System.Data.SqlClient;

namespace SqlDbAccessConsoleTest {
  class Program {
    static void Main(string[] args) {

      Trace.IndentSize = 2;
      //SqlDatabase.DefaultServerName = "gnlieav1";
      //SqlDatabase.DefaultDatabaseName = "TestDb";
      //SqlDatabase.DefaultUserName = "sa";
      //SqlDatabase.DefaultPassword = "welcome1$";
      TSqlDatabase.DefaultUsePooledConnections = true;
      //SqlDatabase NewDatabase = new SqlDatabase();
      const string CONNECTION_STRING = "server=gnlieav1\\sqlexpress;database=derogation;username=sa;password=welcome1$";

      try {
        using (TSqlDatabase DerogationDatabase = new TSqlDatabase(CONNECTION_STRING)) {
          if (DerogationDatabase.Exists()) {
            DerogationDatabase.Drop();
          }
          DerogationDatabase.Create();
          DerogationDatabase.Schema = new SqlSchema(DerogationDatabase.Name, ".\\resources\\derogation.schema.xml");
          DerogationDatabase.ReadSchema();
          DerogationDatabase.CreateSchema();
          //List<string> Tables = DerogationDatabase.ListTables().ToList();
          //ConsoleExtension.Pause();
          //DerogationDatabase.DropTables();
          //Tables = DerogationDatabase.ListTables().ToList();
          //ConsoleExtension.Pause();
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error in SQL database creation : {0}", ex.Message));
      }

      using (TSqlDatabase TestDB = new TSqlDatabase(CONNECTION_STRING)) {
        foreach (string RecordItem in TestDB.SelectQuery<string>("SELECT Firstname FROM [User]", (R) => { return R.SafeRead<string>("FirstName", ""); })) {
          Trace.WriteLine(RecordItem);
        }
        using (SqlTransaction Transaction = (SqlTransaction)TestDB.StartTransaction()) {
          TestDB.ExecuteNonQuery(Transaction,
            new SqlCommand("INSERT INTO [User] (UserId, FirstName, LastName, Priviledge) VALUES (1,'Luc','Bolly','(none)'"),
            new SqlCommand("INSERT INTO [User] (UserId, FirstName, LastName, Priviledge) VALUES (2,'Herman','Bolly','(none)'")
            );
        }
        foreach (string RecordItem in TestDB.SelectQuery<string>("SELECT Firstname FROM [User]", (R) => { return R.SafeRead<string>("FirstName", ""); })) {
          Trace.WriteLine(RecordItem);
        }
      }
    }
  }
}
