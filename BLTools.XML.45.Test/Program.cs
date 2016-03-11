using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools.XML;

namespace BLTools.XML._45.Test {
  class Program {
    static void Main(string[] args) {

      const string SERVERNAME = ".";
      const string DBNAME = "XMLD";

      //TXmlDatabase.DefaultDatabaseName = DBNAME;
      //TXmlDatabase.DefaultServerName = SERVERNAME;

      using (TXmlDatabase Db = new TXmlDatabase(SERVERNAME, DBNAME)) {
        Trace.WriteLine(string.Format("Database {0} {1}", Db.Name, Db.Exists() ? "exists" : "does not exists"));
        if (Db.Exists()) {
          Trace.WriteLine(string.Format("List of tables for {0}", Db.Name));
          Trace.WriteLine(string.Join("\n", Db.ListTables()));
        }
      }
    }
  }
}
