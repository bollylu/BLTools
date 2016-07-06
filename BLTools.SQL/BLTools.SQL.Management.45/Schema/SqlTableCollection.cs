using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SqlServer.Management.Smo;

namespace BLTools.SQL {
  public class SqlTableCollection : List<SqlTable>, IToXml {

    #region XML Tags
    internal const string TAG_THIS_ELEMENT = "Tables"; 
    #endregion XML Tags

    #region Constructor(s)
    public SqlTableCollection() { }

    public SqlTableCollection(XElement sqlTables) {
      foreach (XElement TableItem in sqlTables.Elements(SqlTable.TAG_THIS_ELEMENT)) {
        this.Add(new SqlTable(TableItem));
      }
    }

    public SqlTableCollection(IEnumerable<SqlTable> sqlTables) {
      foreach (SqlTable TableItem in sqlTables) {
        this.Add(new SqlTable(TableItem));
      }
    }

    public SqlTableCollection(SqlTableCollection sqlTables) {
      foreach (SqlTable TableItem in sqlTables) {
        this.Add(new SqlTable(TableItem));
      }
    } 
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0} table{1}", this.Count, this.Count > 1 ? "s" : "");
      RetVal.AppendFormat(" ({0}", string.Join(", ", this.Select(t => t.Name)));
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      foreach (SqlTable TableItem in this) {
        RetVal.Add(TableItem.ToXml());
      }
      return RetVal;
    } 
    #endregion Converters

    

  }
}
