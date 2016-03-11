using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SqlServer.Management.Smo;

namespace BLTools.SQL {
  public class SqlDefault : IToXml {

    #region XML Tags
    internal const string TAG_THIS_ELEMENT = "Default";
    internal const string TAG_ATTRIBUTE_NAME = "Name";
    internal const string TAG_ATTRIBUTE_VALUE = "Value";
    #endregion XML Tags

    #region Public properties
    public string Name { get; set; }
    public string Value { get; set; }
    #endregion Public properties

    #region Constructor(s)
    public SqlDefault() {
      Name = "";
      Value = "";
    }

    public SqlDefault(XElement sqlDefault)
      : this() {
      #region Validate parameters
      if (sqlDefault == null) {
        string Msg = "Unable to create an SqlDefault from a null XElement";
        Trace.WriteLine(Msg);
        throw new ArgumentNullException("sqlDefault", Msg);
      }
      #endregion Validate parameters
      Name = sqlDefault.SafeReadAttribute<string>(TAG_ATTRIBUTE_NAME, "");
      Value = sqlDefault.SafeReadAttribute<string>(TAG_ATTRIBUTE_VALUE, "");
    }

    public SqlDefault(SqlDefault sqlDefault)
      : this() {
        Name = sqlDefault.Name;
        Value = sqlDefault.Value;
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0}", Name);
      RetVal.AppendFormat(" : {0}", Value);
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_NAME, Name);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_VALUE, Value);
      return RetVal;
    }
    #endregion Converters

    #region Public methods
    public void Create(Database database) {
      Trace.WriteLine(string.Format("Creation of default {0}", this.ToString()));
      Default NewDefault = new Default(database, Name);
      NewDefault.TextHeader = string.Format("CREATE DEFAULT [{0}] AS ", Name);
      NewDefault.TextBody = string.Format("'{0}'", Value);
      NewDefault.Create();
    }
    #endregion Public methods

  }
}
