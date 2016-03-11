using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SqlServer.Management.Smo;

namespace BLTools.SQL {
  public class SqlDefaultCollection : List<SqlDefault>, IToXml {

    internal const string TAG_THIS_ELEMENT = "Defaults";

    #region Constructor(s)
    public SqlDefaultCollection() { }

    public SqlDefaultCollection(SqlDefaultCollection sqlDefaults) {
      if (sqlDefaults == null) {
        Trace.WriteLine("Unable to create a SqlDefaultCollection from a null SqlDefaultCollection");
        return;
      }
      foreach (SqlDefault DefaultItem in sqlDefaults) {
        Add(new SqlDefault(DefaultItem));
      }
    }

    public SqlDefaultCollection(IEnumerable<SqlDefault> sqlDefaults) {
      if (sqlDefaults == null) {
        Trace.WriteLine("Unable to create a SqlDefaultCollection from a null SqlDefaultCollection");
        return;
      }
      foreach (SqlDefault DefaultItem in sqlDefaults) {
        Add(new SqlDefault(DefaultItem));
      }
    }

    public SqlDefaultCollection(IEnumerable<XElement> sqlDefaults) {
      if (sqlDefaults == null) {
        Trace.WriteLine("Unable to create a SqlDefaultCollection from a null SqlDefaultCollection");
        return;
      }
      foreach (XElement DefaultItem in sqlDefaults) {
        Add(new SqlDefault(DefaultItem));
      }
    }

    public SqlDefaultCollection(XElement sqlDefaults) {
      if (sqlDefaults == null) {
        Trace.WriteLine("Unable to create a SqlDefaultCollection from a null SqlDefaultCollection");
        return;
      }
      foreach (XElement DefaultItem in sqlDefaults.Elements(SqlDefault.TAG_THIS_ELEMENT)) {
        Add(new SqlDefault(DefaultItem));
      }
    } 
    #endregion Constructor(s)

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0} default{1}", this.Count, this.Count > 1 ? "s" : "");
      RetVal.AppendFormat(" ({0}", string.Join(", ", this.Select(t => t.Name)));
      return RetVal.ToString();
    }
    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      foreach (SqlDefault DefaultItem in this) {
        RetVal.Add(DefaultItem.ToXml());
      }
      return RetVal;
    }

    public void Create(Database database) {
      foreach (SqlDefault DefaultItem in this) {
        DefaultItem.Create(database);
      }
    }

  }
}
