using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SqlServer.Management.Smo;

namespace BLTools.SQL {
  public class SqlIndexCollection : List<SqlIndex> {

    #region XML Tags
    internal const string TAG_THIS_ELEMENT = "Indexes"; 
    #endregion XML Tags

    #region Constructor(s)
    public SqlIndexCollection() { }

    public SqlIndexCollection(XElement sqlIndexes) {
      foreach (XElement IndexItem in sqlIndexes.Elements(SqlIndex.TAG_THIS_ELEMENT)) {
        this.Add(new SqlIndex(IndexItem));
      }
    }

    public SqlIndexCollection(IEnumerable<SqlIndex> sqlIndexes) {
      foreach (SqlIndex IndexItem in sqlIndexes) {
        this.Add(new SqlIndex(IndexItem));
      }
    }

    public SqlIndexCollection(SqlIndexCollection sqlIndexes) {
      foreach (SqlIndex IndexItem in sqlIndexes) {
        this.Add(new SqlIndex(IndexItem));
      }
    }

    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0} index{1}", this.Count, this.Count > 1 ? "s" : "");
      RetVal.AppendFormat(" ({0})", string.Join(", ", this.Select(t => t.Name)));
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      foreach (SqlIndex IndexItem in this) {
        RetVal.Add(IndexItem.ToXml());
      }
      return RetVal;
    }
    #endregion Converters

    #region Public methods
    public void Create(Table table) {
      foreach (SqlIndex IndexItem in this) {
        IndexItem.Create(table);
      }
    }  
    #endregion Public methods
    
  }
}
