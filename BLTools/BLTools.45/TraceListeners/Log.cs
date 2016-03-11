using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BLTools {
  public static class Log {

    public static ErrorLevel DebugLevel = ErrorLevel.Info;

    public static void Write(string message) {
      Write(message, ErrorLevel.Info);
    }
    public static void Write(string message, ErrorLevel severity) {
      if ( DebugLevel >= severity ) {
        Trace.WriteLine(message, severity.ToString());
      }
    }
  }
}
