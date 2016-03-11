using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.ADS {
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

  public class TRecordCacheCollection : List<TRecordCache> {

    private int CurrentRecord = -1;

    public TRecordCacheCollection(IDataReader dataReader) {
      Reset();
      while (dataReader.Read()) {
        Add(new TRecordCache(dataReader));
      }
    }

    public void Reset() {
      CurrentRecord = 0;
    }

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

    public TRecordCache Peek() {
      if (this.Count == 0) {
        return null;
      }
      if (CurrentRecord+1 < this.Count) {
        return this[CurrentRecord+1];
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
