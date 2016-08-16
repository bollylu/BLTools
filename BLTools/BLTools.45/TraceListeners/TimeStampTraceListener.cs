using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BLTools {

  /// <summary>
  /// Act as a trace with timestamp in front of text values
  /// </summary>
  public class TimeStampTraceListener : TExtendedTraceListenerBase {

    /// <summary>
    /// Defines the encoding used to write the log
    /// </summary>
    public Encoding ListenerEncoding { get; set; } = Encoding.Default;

    #region Constructors
    /// <summary>
    /// Create a new TimeStampTraceListener using a filename. It can be named.
    /// </summary>
    /// <param name="filename">The name of the file that will contain the log (including path name)</param>
    /// <param name="name">The name of the TraceListener or empty for anonymous</param>
    public TimeStampTraceListener(string filename, string name = "")
      : base() {
      if (string.IsNullOrWhiteSpace(filename)) {
        throw new ArgumentNullException("Unable to create a TimeStampTraceListener with a null or empty filename");
      }

      // Verify that the path exists: if not, then creates it
      try {
        string Pathname = Path.GetDirectoryName(filename).Trim();
        if ((Pathname != "") && (!Directory.Exists(Pathname))) {
          Directory.CreateDirectory(Pathname);
        }
      } catch (Exception ex) {
        throw new ApplicationException($"Unable to create folder \"{filename}\" for the trace system : {ex.Message}", ex.InnerException);
      }

      this.LogFileName = filename;
      this.Name = name;
    }

    #endregion Constructors

    #region Public methods
    /// <summary>
    /// Writes a message into log file with severity. If the message contains '\n', the line will be splitted.
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="severity">the severity of the message</param>
    public override void WriteLine(string message, string severity) {
      #region Validate parameters
      if (message == null) {
        return;
      }
      if (severity == null) {
        severity = Severity.Information;
      }
      #endregion Validate parameters
      string IndentSpace = base.NeedIndent ? new string(' ', base.IndentLevel * base.IndentSize) : "";

      lock (syncIt) {
        if (message.Contains(Environment.NewLine)) {
          string ModifiedMessage = message.Replace(Environment.NewLine, CRLF_REPLACEMENT.ToString());
          string[] AllLines = ModifiedMessage.Split(CRLF_REPLACEMENT);
          foreach (string LineItem in AllLines) {
            File.AppendAllText(this.LogFileName, _BuildLogLine(LineItem, severity, IndentSpace), ListenerEncoding);
          }
        } else {
          File.AppendAllText(this.LogFileName, _BuildLogLine(message, severity, IndentSpace), ListenerEncoding);
        }
      }
    }

    /// <summary>
    /// Delete the current log file.
    /// A new log file will be created the first time a new log line is added
    /// </summary>
    public void ResetLog() {
      lock (syncIt) {
        try {
          File.Delete(this.LogFileName);
        } catch (Exception ex) {
          Trace.WriteLine($"Unable to ResetLog for {LogFileName} : {ex.Message}");
        }
      }
    }

    /// <summary>
    /// Will close the current log file, and rename it as oldfilename + datetime + .log.
    /// A new log file will be created the first time a new log line is added
    /// </summary>
    public string Rollover() {
      return Rollover($"{Path.GetFileNameWithoutExtension(this.LogFileName)}+{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}{Path.GetExtension(this.LogFileName)}");
    }

    /// <summary>
    /// Will close the current log file, and rename it as newName.
    /// A new log file will be created the first time a new log line is added
    /// </summary>
    /// <param name="newName">Name of the renamed (only the filename+extension part, not the path) version</param>
    public string Rollover(string newName) {
      #region Validate parameters
      if (newName == null) {
        string Msg = $"Unable to rollover {LogFileName} because the new name is null";
        Trace.WriteLine(Msg, Severity.Fatal);
        throw new ArgumentNullException("newName", Msg);
      }
      #endregion Validate parameters
      lock (syncIt) {
        try {
          string Destination = Path.Combine(Path.GetDirectoryName(this.LogFileName), newName);
          File.Move(this.LogFileName, Destination);
          return Destination;
        } catch (Exception ex) {
          Trace.WriteLine($"Unable to Rollover for \"{LogFileName}\" to \"{newName}\" : {ex.Message}");
          return null;
        }
      }
    }
    #endregion Public methods

  }
}
