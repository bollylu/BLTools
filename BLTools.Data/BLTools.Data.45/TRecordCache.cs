using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Data {
  /// <summary>
  /// Cache the content of a record from the IDataReader
  /// </summary>
  public class TRecordCache {

    public Dictionary<string, object> Values { get; set; }

    public TRecordCache(IDataReader dataReader) {
      Values = new Dictionary<string, object>(dataReader.FieldCount);
      for (int i = 0; i < dataReader.FieldCount; i++) {
        Values.Add(dataReader.GetName(i), dataReader.GetValue(i));
      }
    }

    public object this[string name] {
      get {
        if (name == null) {
          return null;
        }
        return Values[name];
      }
    }

  }

  /// <summary>
  /// Read the IDataReader content and cache it. You can access content in forward only as for IDataReader,
  /// plus you can peek the next record without advancing the current record pointer
  /// </summary>
  public class TRecordCacheCollection : List<TRecordCache> {

    private int CurrentRecord = -1;

    DataTable InternalTable;

    public TRecordCacheCollection() {}

    public TRecordCacheCollection(IDataReader dataReader) {
      Reset();
      while (dataReader.Read()) {
        Add(new TRecordCache(dataReader));
      }
    }

    public void Load(IDataReader dataReader) {
      InternalTable = new DataTable();
      InternalTable.BeginLoadData();

      InternalTable.Load(dataReader);

      InternalTable.EndLoadData();
    }

    /// <summary>
    /// Set the current record pointer to begining of collection
    /// </summary>
    public void Reset() {
      CurrentRecord = 0;
    }

    /// <summary>
    /// Get the next record (if available) and move the current record pointer
    /// </summary>
    /// <returns>the next record or null if not available</returns>
    public TRecordCache Read() {
      if (this.Count == 0) {
        return null;
      }
      CurrentRecord++;
      if (CurrentRecord < this.Count) {
        return this[CurrentRecord];
      } else {
        return null;
      }
    }

    /// <summary>
    /// Get the next record (if available) without moving the current record pointer
    /// </summary>
    /// <returns>The next record or null if not available</returns>
    public TRecordCache Peek() {
      if (this.Count == 0) {
        return null;
      }
      if ((CurrentRecord + 1) < this.Count) {
        return this[CurrentRecord + 1];
      } else {
        return null;
      }
    }

    public object this[string name] {
      get {
        if (name == null) {
          return null;
        }
        return this[CurrentRecord].Values[name];
      }
    }
  }
}
