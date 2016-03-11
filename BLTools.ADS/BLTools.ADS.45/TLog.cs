using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BLTools;

namespace BLTools.ADS {
  internal class TLog {
    private TimeStampTraceListener Listener;
    internal TLog() {
      string DefaultLogFile;
      if (Assembly.GetEntryAssembly() != null && Assembly.GetEntryAssembly().GetName() != null) {
       DefaultLogFile= string.Format("c:\\logs\\{0}.log", Assembly.GetEntryAssembly().GetName().Name);
       Listener = new TimeStampTraceListener(DefaultLogFile);
      }
    }
    internal TLog(string filename) {
      if (string.IsNullOrWhiteSpace(filename)) {
        Listener = null;
      } else {
        Listener = new TimeStampTraceListener(filename);
      }
    }

    internal void Write(string message) {
      Write(message, ErrorLevel.Info);
    }
    internal void Write(string message, ErrorLevel severity) {
      if (Listener != null) {
        Listener.WriteLine(message, severity.ToString());
      }
    }
  }
}
