using System;
using System.Diagnostics;

namespace BLTools {
  public class NullTraceListener : DefaultTraceListener {
    
    public NullTraceListener() {}
    public NullTraceListener(string name) {
      this.Name = name;
    }
    public override void Write(string sValue) {}
    public override void WriteLine(string sValue) {}


  }
}
