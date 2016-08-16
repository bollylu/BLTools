using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BLTools;
using Microsoft.SqlServer.Management.Smo;

namespace BLTools.SQL.Management {
  public class SqlTable : IToXml {

    #region XML Tags
    internal const string TAG_THIS_ELEMENT = "Table";
    internal const string TAG_ATTRIBUTE_NAME = "Name";
    #endregion XML Tags

    #region Public properties
    public string Name { get; set; }
    public SqlColumnCollection Columns { get; set; }
    public SqlIndexCollection Indexes { get; set; }
    public SqlConstraintCollection Constraints { get; set; }
    #endregion Public properties

    #region Constructor(s)
    public SqlTable() {
      Name = "";
      Columns = new SqlColumnCollection();
      Indexes = new SqlIndexCollection();
      Constraints = new SqlConstraintCollection();
    }

    public SqlTable(XElement table)
      : this() {
      Name = table.SafeReadAttribute<string>(TAG_ATTRIBUTE_NAME, "");
      Columns = new SqlColumnCollection(table.SafeReadElement(SqlColumnCollection.TAG_THIS_ELEMENT));
      if (table.Elements().Any(e => e.Name == SqlIndexCollection.TAG_THIS_ELEMENT)) {
        Indexes = new SqlIndexCollection(table.SafeReadElement(SqlIndexCollection.TAG_THIS_ELEMENT));
      }
      if (table.Elements().Any(e => e.Name == SqlConstraintCollection.TAG_THIS_ELEMENT)) {
        Constraints = new SqlConstraintCollection(table.SafeReadElement(SqlConstraintCollection.TAG_THIS_ELEMENT));
      }
    }

    public SqlTable(SqlTable table)
      : this() {
      Name = table.Name;
      Columns = new SqlColumnCollection(table.Columns);
      Indexes = new SqlIndexCollection(table.Indexes);
      Constraints = new SqlConstraintCollection(table.Constraints);
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0}", Name);
      RetVal.AppendFormat(", {0} column(s)", Columns.Count);
      RetVal.AppendFormat(", {0} index(es)", Indexes.Count);
      RetVal.AppendFormat(", {0} constraint(s)", Constraints.Count);
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_NAME, Name);
      RetVal.Add(Columns.ToXml());
      RetVal.Add(Indexes.ToXml());
      RetVal.Add(Constraints.ToXml());
      return RetVal;
    }
    #endregion Converters

    #region Public methods
    public void Create(Database database) {
      #region Validate parameters
      if (database == null) {
        string Msg = "Unable to create a Table without a valid database";
        Trace.WriteLine(Msg);
        throw new ArgumentNullException("database", Msg);
      }
      #endregion Validate parameters

      Trace.WriteLine(string.Format("Creation of table \"{0}\"", Name));
      Trace.Indent();
      string CompletionMessage = "";
      try {
        Table NewTable = new Table(database, Name);
        foreach (SqlColumn ColumnItem in Columns) {
          Trace.WriteLine(string.Format("Adding column {0}", ColumnItem.ToString()));
          NewTable.Columns.Add(ColumnItem.Instanciate(NewTable));
        }
        NewTable.Create();
        CompletionMessage = "Done.";

        if (Indexes.Count > 0) {
          Trace.WriteLine(string.Format("Adding {0} index{1}", Indexes.Count, Indexes.Count > 1 ? "es" : ""));
          Trace.Indent();
          string IndexCompletionMessage = "";
          try {
            Indexes.Create(NewTable);
            IndexCompletionMessage = "Done.";
          } catch (Exception ex) {
            IndexCompletionMessage = string.Format("Failed: {0}", ex.Message);
          } finally {
            Trace.Unindent();
            Trace.WriteLine(IndexCompletionMessage);
          }
        } else {
          Trace.WriteLine("No index");
        }

        if (Constraints.Count > 0) {
          Trace.WriteLine(string.Format("Adding {0} constraint{1}", Constraints.Count, Constraints.Count > 1 ? "s" : ""));
          Trace.Indent();
          string ConstraintCompletionMessage = "";
          try {
            Constraints.Create(NewTable);
            ConstraintCompletionMessage = "Done.";
          } catch (Exception ex) {
            ConstraintCompletionMessage = string.Format("Failed: {0}", ex.Message);
          } finally {
            Trace.Unindent();
            Trace.WriteLine(ConstraintCompletionMessage);
          }
        } else {
          Trace.WriteLine("No constraint");
        }

      } catch (Exception ex) {
        CompletionMessage = string.Format("Failed: {0}", ex.Message);
      } finally {
        Trace.Unindent();
        Trace.WriteLine(CompletionMessage);
      }

    }
    #endregion Public methods

  }
}
