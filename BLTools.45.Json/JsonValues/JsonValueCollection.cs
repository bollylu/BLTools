using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  internal class JsonValueCollection : List<IJsonValue>, IDisposable {

    private object _LockCollection = new object();

    public void Dispose() {
      lock ( _LockCollection ) {
        foreach ( IJsonValue ValueItem in this ) {
          ValueItem.Dispose();
        }
      }
    }

  }
}
