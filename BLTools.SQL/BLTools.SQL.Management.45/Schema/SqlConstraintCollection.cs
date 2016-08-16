using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SqlServer.Management.Smo;

namespace BLTools.SQL.Management {
  public class SqlConstraintCollection : List<SqlConstraint>, IToXml {

     #region XML Tags
    internal const string TAG_THIS_ELEMENT = "Constraints";
    #endregion XML Tags

    #region Constructor(s)
    public SqlConstraintCollection() { }

    public SqlConstraintCollection(XElement constraints) {
      foreach (XElement ConstraintItem in constraints.Elements(SqlConstraint.TAG_THIS_ELEMENT)) {
        this.Add(new SqlConstraint(ConstraintItem));
      }
    }

    public SqlConstraintCollection(IEnumerable<SqlConstraint> constraints) {
      foreach (SqlConstraint ConstraintItem in constraints) {
        this.Add(new SqlConstraint(ConstraintItem));
      }
    }

    public SqlConstraintCollection(SqlConstraintCollection constraints) {
      foreach (SqlConstraint ConstraintItem in constraints) {
        this.Add(new SqlConstraint(ConstraintItem));
      }
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0} constraint{1}", this.Count, this.Count > 1 ? "s" : "");
      RetVal.AppendFormat(" ({0}", string.Join(", ", this.Select(t => t.Name)));
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      foreach (SqlConstraint ConstraintItem in this) {
        RetVal.Add(ConstraintItem.ToXml());
      }
      return RetVal;

    }
    #endregion Converters

    #region Public methods
    public void Create(Table table) {
      foreach (SqlConstraint ConstraintItem in this) {
        ConstraintItem.Create(table);
      }
    }
    #endregion Public methods
  }
}
