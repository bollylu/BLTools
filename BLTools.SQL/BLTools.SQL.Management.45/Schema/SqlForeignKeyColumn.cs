using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLTools.SQL {
  public class SqlForeignKeyColumn {

    #region XML Tags
    internal const string TAG_THIS_ELEMENT = "ForeignKeyColumn";
    internal const string TAG_ATTRIBUTE_NAME = "Name";
    internal const string TAG_ATTRIBUTE_PARENT_NAME = "ParentName"; 
    #endregion XML Tags

    #region Public properties
    public string Name { get; set; }
    public string ParentName { get; set; } 
    #endregion Public properties

    #region Constructor(s)
    public SqlForeignKeyColumn() {
      Name = "";
      ParentName = "";
    }

    public SqlForeignKeyColumn(XElement foreignKeyColumn)
      : this() {
      #region Validate parameters
      if (foreignKeyColumn == null) {
        string Msg = "Unable to create a ForeignKeyColumn from a null XElement";
        Trace.WriteLine(Msg);
        throw new ArgumentNullException("foreignKeyColumn", Msg);
      }
      #endregion Validate parameters
      Name = foreignKeyColumn.SafeReadAttribute<string>(TAG_ATTRIBUTE_NAME, "");
      ParentName = foreignKeyColumn.SafeReadAttribute<string>(TAG_ATTRIBUTE_PARENT_NAME, "");
    }

    public SqlForeignKeyColumn(SqlForeignKeyColumn foreignKeyColumn)
      : this() {
      Name = foreignKeyColumn.Name;
      ParentName = foreignKeyColumn.ParentName;
    } 
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0}", Name);
      RetVal.AppendFormat("=>{0}", ParentName);
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_NAME, Name);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_PARENT_NAME, ParentName);
      return RetVal;
    } 
    #endregion Converters

  }
}
