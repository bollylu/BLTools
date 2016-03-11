using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BLTools;

namespace BLTools.Data {

  public interface IData {
    int KeyId { get; set; }
  }

  public interface IData<R> {
    int KeyId { get; set; }
  }

  public interface IDataProvider {
    IData Select(int keyid);
    IData SelectRecord(string query);
    IData Create(IData record);
    bool Update(IData record);
    bool Delete(IData record);
    IEnumerable<IData> SelectAll();
    IEnumerable<IData> SelectQuery(string query);
  }

  public interface IDataProvider<R> {
    IData<R> Select(int keyid);
    IData<R> SelectRecord(string query);
    IData<R> Create(IData<R> record);
    bool Update(IData<R> record);
    bool Delete(IData<R> record);
    IEnumerable<IData<R>> SelectAll();
    IEnumerable<IData<R>> SelectQuery(string query);
  }
}
