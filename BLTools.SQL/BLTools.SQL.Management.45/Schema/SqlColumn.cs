using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BLTools;
using Microsoft.SqlServer.Management.Smo;

namespace BLTools.SQL {
  public class SqlColumn : IToXml {

    #region XML Tags
    internal const string TAG_THIS_ELEMENT = "Column";
    internal const string TAG_ATTRIBUTE_NAME = "Name";
    internal const string TAG_ATTRIBUTE_LENGTH = "Length";
    internal const string TAG_ATTRIBUTE_DATATYPE = "DataType";
    internal const string TAG_ATTRIBUTE_NULLABLE = "IsNullable";
    internal const string TAG_ATTRIBUTE_IDENTITY = "IsIdentity";
    internal const string TAG_ATTRIBUTE_IDENTITY_SEED = "IdentitySeed";
    internal const string TAG_ATTRIBUTE_IDENTITY_INCREMENT = "IdentityIncrement";
    internal const string TAG_ATTRIBUTE_DEFAULT_VALUE = "Default";
    #endregion XML Tags

    #region Public properties
    public string Name { get; set; }
    public DataType SqlDataType { get; set; }
    public int Length { get; set; }
    public bool IsNullable { get; set; }
    public bool IsIdentity { get; set; }
    public int IdentitySeed { get; set; }
    public int IdentityIncrement { get; set; }
    public bool HasDefaultValue { get; set; }
    public string DefaultValue {
      get {
        return _DefaultValue;
      }
      set{
        HasDefaultValue = value != "";
        _DefaultValue = value;
      }
    }
    private string _DefaultValue;
    #endregion Public properties

    #region Constructor(s)
    public SqlColumn() {
      Name = "";
      SqlDataType = null;
      Length = 0;
      IsNullable = true;
      IsIdentity = false;
      IdentitySeed = 0;
      IdentityIncrement = 1;
      HasDefaultValue = false;
      DefaultValue = "";
    }

    public SqlColumn(XElement column)
      : this() {
      #region Validate parameters
      if (column == null) {
        string Msg = "Unable to create an SqlColumn from a null XElement";
        Trace.WriteLine(Msg);
        throw new ArgumentNullException("column", Msg);
      }
      #endregion Validate parameters
      Name = column.SafeReadAttribute<string>(TAG_ATTRIBUTE_NAME, "");
      if (column.Attributes().Any(a => a.Name == TAG_ATTRIBUTE_LENGTH)) {
        Length = column.SafeReadAttribute<int>(TAG_ATTRIBUTE_LENGTH, 0);
      }
      SqlDataType = SqlHelper.TextToSqlDataType(column.SafeReadAttribute<string>(TAG_ATTRIBUTE_DATATYPE, ""), Length);
      if (column.Attributes().Any(a => a.Name == TAG_ATTRIBUTE_NULLABLE)) {
        IsNullable = column.SafeReadAttribute<bool>(TAG_ATTRIBUTE_NULLABLE, true);
      }
      if (column.Attributes().Any(a => a.Name == TAG_ATTRIBUTE_IDENTITY)) {
        IsIdentity = column.SafeReadAttribute<bool>(TAG_ATTRIBUTE_IDENTITY, false);
        IdentitySeed = column.SafeReadAttribute<int>(TAG_ATTRIBUTE_IDENTITY_SEED, 0);
        IdentityIncrement = column.SafeReadAttribute<int>(TAG_ATTRIBUTE_IDENTITY_INCREMENT, 1);
      }
      if (column.Attributes().Any(a => a.Name == TAG_ATTRIBUTE_DEFAULT_VALUE)) {
        DefaultValue = column.SafeReadAttribute<string>(TAG_ATTRIBUTE_DEFAULT_VALUE, "");
        if (DefaultValue != "") {
          HasDefaultValue = true;
        }
      }
    }

    public SqlColumn(SqlColumn column)
      : this() {
      Name = column.Name;
      SqlDataType = column.SqlDataType;
      Length = column.Length;
      IsNullable = column.IsNullable;
      IsIdentity = column.IsIdentity;
      IdentitySeed = column.IdentitySeed;
      IdentityIncrement = column.IdentityIncrement;
      HasDefaultValue = column.HasDefaultValue;
      DefaultValue = column.DefaultValue;
    }
    #endregion Constructor(s)

    #region Converters
    static private List<string> NeedLength = new List<string>() {
      "char",
      "varchar",
      "nvarchar",
      "binary"
    };

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0}", Name);
      RetVal.AppendFormat(", {0}", SqlDataType.SqlDataType.ToString());
      if (NeedLength.Contains(SqlDataType.SqlDataType.ToString().ToLower())) {
        RetVal.AppendFormat(", {0}", Length);
      }
      RetVal.AppendFormat(", {0}", IsNullable ? "NULL" : "NOT NULL");
      if (IsIdentity) {
        RetVal.AppendFormat(", Identity({0},{1})", IdentitySeed, IdentityIncrement);
      }
      if (HasDefaultValue) {
        RetVal.AppendFormat(", Default={0}", DefaultValue);
      }
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_NAME, Name);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_DATATYPE, SqlDataType.Name);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_LENGTH, Length);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_NULLABLE, IsNullable);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_IDENTITY, IsIdentity);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_IDENTITY_SEED, IdentitySeed);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_IDENTITY_INCREMENT, IdentityIncrement);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_DEFAULT_VALUE, DefaultValue);
      return RetVal;
    }
    #endregion Converters

    #region Public methods
    public Column Instanciate(Table table) {
      Column RetVal = new Column();
      RetVal.Parent = table;
      RetVal.Name = Name;
      RetVal.DataType = SqlDataType;
      RetVal.Nullable = IsNullable;
      RetVal.Identity = IsIdentity;
      RetVal.IdentitySeed = IdentitySeed;
      RetVal.IdentityIncrement = IdentityIncrement;
      if (HasDefaultValue) {
        RetVal.AddDefaultConstraint();
        RetVal.DefaultConstraint.Text = DefaultValue;
      }
      return RetVal;
    }
    #endregion Public methods

  }
}
