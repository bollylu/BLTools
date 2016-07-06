using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SqlServer.Management.Smo;

namespace BLTools.SQL {
  public class SqlSchema : IToXml {

    #region XML Tags
    internal const string TAG_THIS_ELEMENT = "Schema";
    internal const string TAG_ATTRIBUTE_NAME = "Name";
    #endregion XML Tags

    #region Public properties
    public string Name { get; set; }
    public string Location { get; set; }
    public SqlTableCollection Tables { get; set; }
    #endregion Public properties

    #region Constructor(s)
    public SqlSchema() {
      Name = "";
      Location = "";
      Tables = new SqlTableCollection();
    }

    public SqlSchema(string name, string location)
      : this() {
      Name = name;
      Location = location;
    }

    public SqlSchema(SqlSchema schema)
      : this() {
      Name = schema.Name;
      Location = Location;
      Tables = new SqlTableCollection(schema.Tables);
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0}", Name);
      RetVal.AppendFormat(" ({0} table{1})", Tables.Count, Tables.Count > 0 ? "s" : "");
      return RetVal.ToString();
    }
    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_NAME, Name);
      RetVal.Add(Tables.ToXml());
      return RetVal;
    }
    #endregion Converters

    #region Public methods
    public void ReadFromXml() {
      ReadFromXml(Location);
    }
    public void ReadFromXml(string location) {
      #region Validate parameters
      if (!File.Exists(location)) {
        Trace.WriteLine(string.Format("Unable to read schema : file is missing or access is denied."));
        return;
      }
      #endregion Validate parameters

      XDocument SchemaFile = null;
      try {
        SchemaFile = XDocument.Load(location);
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading schema : {0}", ex.Message));
        return;
      }

      ParseXDocument(SchemaFile);
    }
    public void ReadFromXml(Stream stream) {
      #region Validate parameters
      if (stream==null) {
        Trace.WriteLine(string.Format("Unable to read schema : data stream is null"));
        return;
      }
      #endregion Validate parameters

      XDocument SchemaFile = null;
      try {
        SchemaFile = XDocument.Load(stream);
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading schema : {0}", ex.Message));
        return;
      }

      ParseXDocument(SchemaFile);
    }
    public void Create(Database database) {
      foreach (SqlTable TableItem in Tables) {
        TableItem.Create(database);
        if (OnSchemaTableCreated != null) {
          OnSchemaTableCreated(this, new SchemaTableCreatedEventArgs(TableItem));
        }
      }
      if (OnSchemaCreated != null) {
        OnSchemaCreated(this, EventArgs.Empty);
      }
    }
    #endregion Public methods

    #region Private methods
    private void ParseXDocument(XDocument SchemaFile) {
      XElement Root = SchemaFile.Root;
      Tables = new SqlTableCollection(Root.Element(SqlTableCollection.TAG_THIS_ELEMENT));
    } 
    #endregion Private methods

    #region Events
    public event EventHandler OnSchemaCreated;
    public event EventHandler<SchemaTableCreatedEventArgs> OnSchemaTableCreated;
    #endregion Events
  }
}
