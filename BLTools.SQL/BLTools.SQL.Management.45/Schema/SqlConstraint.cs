using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SqlServer.Management.Smo;

namespace BLTools.SQL {
  public class SqlConstraint : IToXml {

    #region XML Tags
    internal const string TAG_THIS_ELEMENT = "Constraint";
    internal const string TAG_ATTRIBUTE_NAME = "Name";
    internal const string TAG_ATTRIBUTE_PARENT_TABLE = "ParentTable";
    #endregion XML Tags

    #region Public properties
    public string Name { get; set; }
    public string ParentTable { get; set; }
    public SqlForeignKeyColumn ForeignKey { get; set; }
    #endregion Public properties

    #region Constructor(s)
    public SqlConstraint() {
      Name = "";
      ParentTable = "";
      ForeignKey = new SqlForeignKeyColumn();
    }

    public SqlConstraint(XElement constraint)
      : this() {
      #region Validate parameters
      if (constraint == null) {
        string Msg = "Unable to create an SqlConstraint from a null XElement";
        Trace.WriteLine(Msg);
        throw new ArgumentNullException("constraint", Msg);
      }
      #endregion Validate parameters
      Name = constraint.SafeReadAttribute<string>(TAG_ATTRIBUTE_NAME, "");
      ParentTable = constraint.SafeReadAttribute<string>(TAG_ATTRIBUTE_PARENT_TABLE, "");
      ForeignKey = new SqlForeignKeyColumn(constraint.Element(SqlForeignKeyColumn.TAG_THIS_ELEMENT));
    }

    public SqlConstraint(SqlConstraint constraint)
      : this() {
      Name = constraint.Name;
      ParentTable = constraint.ParentTable;
      ForeignKey = new SqlForeignKeyColumn(constraint.ForeignKey);
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0}", Name);
      RetVal.AppendFormat(", {0} => {1}.{2}", ForeignKey.Name, ParentTable, ForeignKey.ParentName);
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_NAME, Name);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_PARENT_TABLE, ParentTable);
      RetVal.Add(ForeignKey.ToXml());
      return RetVal;
    }
    #endregion Converters

    #region Public methods
    public void Create(Table table) {
      Trace.WriteLine(string.Format("Creation of {0}", this.ToString()));
      ForeignKey NewFK = new ForeignKey(table, Name);
      NewFK.Columns.Add(new ForeignKeyColumn(NewFK, ForeignKey.Name, ForeignKey.ParentName));
      NewFK.ReferencedTable = ParentTable;
      NewFK.Create();
    }
    #endregion Public methods

  }
}
