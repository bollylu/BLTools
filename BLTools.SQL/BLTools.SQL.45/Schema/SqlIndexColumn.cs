using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLTools.SQL {
  public class SqlIndexColumn : IToXml {

    #region XML Tags
    internal const string TAG_THIS_ELEMENT = "IndexColumn";
    internal const string TAG_ATTRIBUTE_NAME = "Name";
    internal const string TAG_ATTRIBUTE_SORTDIRECTION = "SortDirection"; 
    #endregion XML Tags

    #region Public properties
    public string Name { get; set; }
    public SqlIndexColumnSortDirectionEnum SortDirection { get; set; } 
    #endregion Public properties

    #region Constructor(s)
    public SqlIndexColumn() {
      Name = "";
      SortDirection = SqlIndexColumnSortDirectionEnum.Ascending;
    }

    public SqlIndexColumn(XElement indexColumn)
      : this() {
      #region Validate parameters
      if (indexColumn == null) {
        string Msg = "Unable to create an SqlIndexColumn from a null XElement";
        Trace.WriteLine(Msg);
        throw new ArgumentNullException("index", Msg);
      }
      #endregion Validate parameters
      Name = indexColumn.SafeReadAttribute<string>(TAG_ATTRIBUTE_NAME, "");
      if (indexColumn.Attributes().Any(a => a.Name == TAG_ATTRIBUTE_SORTDIRECTION)) {
        SortDirection = (SqlIndexColumnSortDirectionEnum)Enum.Parse(typeof(SqlIndexColumnSortDirectionEnum), indexColumn.SafeReadAttribute<string>(TAG_ATTRIBUTE_SORTDIRECTION, SqlIndexColumnSortDirectionEnum.Ascending.ToString()), true);
      }
    }

    public SqlIndexColumn(SqlIndexColumn indexColumn)
      : this() {
      Name = indexColumn.Name;
      SortDirection = indexColumn.SortDirection;
    } 
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0}", Name);
      RetVal.AppendFormat(":{0}", SortDirection.ToString());
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_NAME, Name);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_SORTDIRECTION, SortDirection.ToString());
      return RetVal;
    } 
    #endregion Converters


  }
}
