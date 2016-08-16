using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools.SQL;

namespace BLTools.SQL.Management {
  public partial class TSqlDatabaseManager : TSqlDatabase {

    public TSqlDatabaseManager() : base() { }

    public TSqlDatabaseManager(string connectionString) : base(connectionString) { }

    public TSqlDatabaseManager(string servername, string databasename, string username, string password) : base(servername, databasename, username, password) { }

    public TSqlDatabaseManager(TSqlDatabase sqlDatabase) : base(sqlDatabase) { }

  }
}
