using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools.SQL.Management {
  public class SchemaTableCreatedEventArgs : EventArgs {
    public SqlTable Table;
    public SchemaTableCreatedEventArgs(SqlTable table) {
      Table = new SqlTable(table);
    }
  }
}
