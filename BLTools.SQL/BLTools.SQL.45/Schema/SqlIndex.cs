using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SqlServer.Management.Smo;

namespace BLTools.SQL {
  public class SqlIndex : IToXml {

    #region XML Tags
    internal const string TAG_THIS_ELEMENT = "Index";
    internal const string TAG_ATTRIBUTE_NAME = "Name";
    internal const string TAG_ATTRIBUTE_PRIMARYKEY = "IsPrimaryKey";
    internal const string TAG_ATTRIBUTE_CLUSTERED = "IsClustered";
    internal const string TAG_ATTRIBUTE_UNIQUE = "IsUnique";
    #endregion XML Tags

    #region Public properties
    public string Name { get; set; }
    public bool IsPrimaryKey { get; set; }
    public bool IsClustered { get; set; }
    public bool IsUnique { get; set; }
    public SqlIndexColumnCollection IndexColumns { get; set; }
    #endregion Public properties

    #region Constructor(s)
    public SqlIndex() {
      Name = "";
      IsPrimaryKey = false;
      IsClustered = false;
      IsUnique = false;
      IndexColumns = new SqlIndexColumnCollection();
    }

    public SqlIndex(XElement index)
      : this() {
      #region Validate parameters
      if (index == null) {
        string Msg = "Unable to create an SqlIndex from a null XElement";
        Trace.WriteLine(Msg);
        throw new ArgumentNullException("index", Msg);
      }
      #endregion Validate parameters
      Name = index.SafeReadAttribute<string>(TAG_ATTRIBUTE_NAME, "");
      if (index.Attributes().Any(a => a.Name == TAG_ATTRIBUTE_PRIMARYKEY)) {
        IsPrimaryKey = index.SafeReadAttribute<bool>(TAG_ATTRIBUTE_PRIMARYKEY, false);
      }
      if (index.Attributes().Any(a => a.Name == TAG_ATTRIBUTE_CLUSTERED)) {
        IsClustered = index.SafeReadAttribute<bool>(TAG_ATTRIBUTE_CLUSTERED, false);
      }
      if (index.Attributes().Any(a => a.Name == TAG_ATTRIBUTE_UNIQUE)) {
        IsUnique = index.SafeReadAttribute<bool>(TAG_ATTRIBUTE_UNIQUE, false);
      }
      IndexColumns = new SqlIndexColumnCollection(index.Element(SqlIndexColumnCollection.TAG_THIS_ELEMENT));
    }

    public SqlIndex(SqlIndex index)
      : this() {
      Name = index.Name;
      IsPrimaryKey = index.IsPrimaryKey;
      IsClustered = index.IsClustered;
      IsUnique = index.IsUnique;
      IndexColumns = new SqlIndexColumnCollection(index.IndexColumns);
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0}", Name);
      RetVal.AppendFormat(", {0}", IsPrimaryKey ? "Primary Key" : "Index");
      if (IsUnique) {
        RetVal.AppendFormat(", {0}", "Unique");
      }
      RetVal.AppendFormat(", {0}", IsClustered ? "Clustered" : "NOT clustered");
      RetVal.AppendFormat(" ({0})", IndexColumns.ToString());
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_NAME, Name);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_PRIMARYKEY, IsPrimaryKey);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_CLUSTERED, IsClustered);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_UNIQUE, IsUnique);
      return RetVal;
    }
    #endregion Converters

    #region Public methods
    public void Create(Table table) {
      Trace.WriteLine(string.Format("Creation of {0}", this.ToString()));
      Index NewIndex = new Index(table, Name);
      NewIndex.IndexKeyType = IsPrimaryKey ? IndexKeyType.DriPrimaryKey : IsUnique ? IndexKeyType.DriUniqueKey : IndexKeyType.None;
      NewIndex.IsClustered = IsClustered;
      NewIndex.IsUnique = IsUnique;
      foreach (SqlIndexColumn ColumnItem in IndexColumns) {
        NewIndex.IndexedColumns.Add(new IndexedColumn(NewIndex, ColumnItem.Name, ColumnItem.SortDirection == SqlIndexColumnSortDirectionEnum.Descending));
      }
      NewIndex.Create();
    }
    #endregion Public methods
  }
}
