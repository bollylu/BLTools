using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Debugging {
  /// <summary>
  /// Helper for Trace management
  /// </summary>
  public class TraceFactory {

    /// <summary>
    /// Default location where log files without path will be created
    /// </summary>
    public static string DefaultLogLocation = "c:\\logs";

    /// <summary>
    /// Add a listener to the Listeners collection of the Trace object. This new listener point to a filename, can be reset and rolled over
    /// </summary>
    /// <param name="filename">Name of the log file</param>
    /// <param name="resetLog">True to reset the log before adding to the listeners</param>
    /// <param name="rollover">True to roll over the log before adding to the listeners</param>
    public static void AddTraceLogFilename(string filename, bool resetLog = false, bool rollover = false) {
      if (string.IsNullOrWhiteSpace(filename)) {
        throw new ArgumentException("filename is missing", "filename");
      }
      TimeStampTraceListener NewTraceListener;
      if (!filename.Contains("\\")) {
        NewTraceListener = new TimeStampTraceListener(Path.Combine(DefaultLogLocation, filename), "(default)");
      } else {
        NewTraceListener = new TimeStampTraceListener(filename, "(default)");
      }
      if (resetLog) {
        NewTraceListener.ResetLog();
      }
      if (rollover) {
        NewTraceListener.Rollover();
      }
      Trace.Listeners.Add(NewTraceListener);
    }

    /// <summary>
    /// Add a listener to the Listeners collection of the Trace object. This new listener is named after the main process filename (i.e. the .exe filename). It can be reset and rolled over
    /// </summary>
    /// <param name="resetLog">True to reset the log before adding to the listeners</param>
    /// <param name="rollover">True to roll over the log before adding to the listeners</param>
    public static void AddTraceDefaultLogFilename(bool resetLog = false, bool rollover = false) {
      TimeStampTraceListener NewTraceListener = new TimeStampTraceListener(GetTraceDefaultLogFilename(), "(default)");
      if (resetLog) {
        NewTraceListener.ResetLog();
      }
      if (rollover) {
        NewTraceListener.Rollover();
      }
      Trace.Listeners.Add(NewTraceListener);
    }

    /// <summary>
    /// Get the name of the default log file (based on application name)
    /// </summary>
    /// <returns>The path and name of the default log file</returns>
    public static string GetTraceDefaultLogFilename() {
      AssemblyName StartupApplication = Assembly.GetEntryAssembly().GetName();
      return Path.Combine(DefaultLogLocation, $"{StartupApplication.Name}.log");
    }

    /// <summary>
    /// Add the console as a listener to the listeners collection of the Trace object
    /// </summary>
    public static void AddTraceConsole() {
      Trace.Listeners.Add(new TimeStampTextWriterTraceListener(Console.Out, "Console"));
    }
  }
}
