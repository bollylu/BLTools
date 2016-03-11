using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools.Security.Authorization {
  public class TSecurityStorageSql : TSecurityStorage {
    public TSecurityStorageSql() {}
    public override void Save() {
      Save(Name);
    }
    public override void Save(string name) { }
  }
}
