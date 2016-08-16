using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BLTools {

  /// <summary>
  /// Act as a trace with timestamp in front of text values
  /// </summary>
  public class TimeStampTextWriterTraceListener : TExtendedTraceListenerBase {

    /// <summary>
    /// Textwriter used to log info
    /// </summary>
    public TextWriter Writer { get; private set; }

    #region Constructors
    /// <summary>
    /// Create a new TimeStampTextWriterTraceListener using a StreamWriter
    /// </summary>
    /// <param name="textWriter">The stream that will receive the log</param>
    /// <param name="name">The name of the TraceListener or empty for anonymous</param>
    public TimeStampTextWriterTraceListener(TextWriter textWriter, string name = "") : base() {
      if (textWriter == null) {
        string Msg = "Unable to create a TimeStampTextWriterTraceListener with a null TextWriter";
        Trace.WriteLine(Msg, Severity.Fatal);
        throw new ArgumentNullException("textWriter", Msg);
      }

      this.Writer = textWriter;
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
            this.Writer.WriteLine(_BuildLogLine(LineItem, severity, IndentSpace));
          }
        } else {
          this.Writer.WriteLine(_BuildLogLine(message, severity, IndentSpace));
        }
      }
    }

    #endregion Public methods

  }
}
