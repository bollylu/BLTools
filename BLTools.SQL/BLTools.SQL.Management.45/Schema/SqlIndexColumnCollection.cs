using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLTools.SQL.Management {
  public class SqlIndexColumnCollection : List<SqlIndexColumn>, IToXml {

    #region XML Tags
    internal const string TAG_THIS_ELEMENT = "IndexColumns"; 
    #endregion XML Tags

    #region Constructor(s)
    public SqlIndexColumnCollection() { }

    public SqlIndexColumnCollection(XElement sqlIndexColumns) {
      if (sqlIndexColumns != null && sqlIndexColumns.Elements().Any(e => e.Name == SqlIndexColumn.TAG_THIS_ELEMENT)) {
        foreach (XElement IndexColumnItem in sqlIndexColumns.Elements(SqlIndexColumn.TAG_THIS_ELEMENT)) {
          this.Add(new SqlIndexColumn(IndexColumnItem));
        }
      }
    }

    public SqlIndexColumnCollection(IEnumerable<SqlIndexColumn> sqlIndexColumns) {
      foreach (SqlIndexColumn IndexColumnItem in sqlIndexColumns) {
        this.Add(new SqlIndexColumn(IndexColumnItem));
      }
    }

    public SqlIndexColumnCollection(SqlIndexColumnCollection sqlIndexColumns) {
      foreach (SqlIndexColumn IndexColumnItem in sqlIndexColumns) {
        this.Add(new SqlIndexColumn(IndexColumnItem));
      }
    } 
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0} index column{1}", this.Count, this.Count > 1 ? "s" : "");
      RetVal.AppendFormat(" ({0})", string.Join(", ", this.Select(t => t.ToString())));
      return RetVal.ToString();
    }


    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      foreach (SqlIndexColumn IndexColumnItem in this) {
        RetVal.Add(IndexColumnItem.ToXml());
      }
      return RetVal;
    } 
    #endregion Converters
  }
}
