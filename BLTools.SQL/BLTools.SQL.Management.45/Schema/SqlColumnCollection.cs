using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SqlServer.Management.Smo;

namespace BLTools.SQL.Management {
  public class SqlColumnCollection : List<SqlColumn>, IToXml {

    #region XML Tags
    internal const string TAG_THIS_ELEMENT = "Columns";
    #endregion XML Tags

    #region Constructor(s)
    public SqlColumnCollection() { }

    public SqlColumnCollection(XElement sqlColumns) {
      foreach (XElement ColumnItem in sqlColumns.Elements(SqlColumn.TAG_THIS_ELEMENT)) {
        this.Add(new SqlColumn(ColumnItem));
      }
    }

    public SqlColumnCollection(IEnumerable<SqlColumn> sqlColumns) {
      foreach (SqlColumn ColumnItem in sqlColumns) {
        this.Add(new SqlColumn(ColumnItem));
      }
    }

    public SqlColumnCollection(SqlColumnCollection sqlColumns) {
      foreach (SqlColumn ColumnItem in sqlColumns) {
        this.Add(new SqlColumn(ColumnItem));
      }
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0} column{1}", this.Count, this.Count > 1 ? "s" : "");
      RetVal.AppendFormat(" ({0}", string.Join(", ", this.Select(t => t.Name)));
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      foreach (SqlColumn ColumnItem in this) {
        RetVal.Add(ColumnItem.ToXml());
      }
      return RetVal;

    }
    #endregion Converters

  }
}
