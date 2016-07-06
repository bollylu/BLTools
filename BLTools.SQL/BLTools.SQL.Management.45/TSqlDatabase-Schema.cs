using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.SQL {
  public partial class TSqlDatabase {

    #region Schema
    public bool ReadSchema() {
      return ReadSchema(Schema.Location);
    }
    public bool ReadSchema(string schemaUri) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(schemaUri)) {
        Trace.WriteLine(string.Format("Schema for database \"{0}\" cannot be read : schema URI is null or empty", DatabaseName), Severity.Error);
        return false;
      }
      if (!File.Exists(schemaUri)) {
        Trace.WriteLine(string.Format("Schema for database \"{0}\" cannot be read : schema URI \"{1}\" is invalid or access is denied", DatabaseName, schemaUri), Severity.Error);
        return false;
      }
      #endregion Validate parameters
      Schema = new SqlSchema(DatabaseName, schemaUri);
      try {
        Schema.ReadFromXml();
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to read schema \"{0}\" for database \"{1}\" : {2}", schemaUri, DatabaseName, ex.Message));
        return false;
      }
    }
    public bool ReadSchema(Stream schemaStream) {
      #region Validate parameters
      if (schemaStream == null) {
        Trace.WriteLine(string.Format("Schema for database \"{0}\" cannot be read : schema stream is null", DatabaseName), Severity.Error);
        return false;
      }
      #endregion Validate parameters
      Schema = new SqlSchema(DatabaseName, "");
      try {
        Schema.ReadFromXml(schemaStream);
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to read schema for database \"{0}\" : {1}", DatabaseName, ex.Message));
        return false;
      }
    }

    public bool CreateSchema() {
      #region Validate parameters
      if (Schema == null) {
        Trace.WriteLine(string.Format("Unable to create schema for database \"{0}\" : schema is null", DatabaseName), Severity.Error);
        return false;
      }
      #endregion Validate parameters
      using (TSqlServer CurrentServer = new TSqlServer(ServerName, UserName, Password)) {
        try {
          Database CurrentDb = CurrentServer.SmoServer.Databases[DatabaseName];
          Schema.Create(CurrentDb);
          return true;
        } catch (Exception ex) {
          Trace.WriteLine(string.Format("Unable to create schema for database \"{0}\" : {1}", DatabaseName, ex.Message));
          return false;
        }
      }
    }
    #endregion Schema

  }
}
