using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  /// <summary>
  /// Add a timestamp, userid, computer in front of the log line
  /// </summary>
  public class TExtendedTraceListenerBase : DefaultTraceListener{

    #region Private variables
    internal static string _UserId = Environment.UserDomainName == "" ? Environment.UserName : $"{Environment.UserDomainName}\\{Environment.UserName}";
    internal static string _Computer = Environment.MachineName;
    
    /// <summary>
    /// If the userid column has to be displayed, indicate its size in chars (min = 8, max = 50)
    /// </summary>
    public static int UserIdColSize {
      get {
        return _UserIdColSize;
      }
      set {
        _UserIdColSize = Math.Max(8, Math.Min(50, value));
      }
    }
    internal static int _UserIdColSize;

    internal static string _UserIdColContent {
      get {
        if (_UserId.Length >= UserIdColSize) {
          return _UserId.Left(UserIdColSize);
        } else {
          return _UserId.PadRight(UserIdColSize, '.');
        }
      }
    }

    
    /// <summary>
    /// If the computer column has to be displayed, indicate its size in chars (min = 8, max = 50)
    /// </summary>
    public static int ComputerColSize {
      get {
        return _ComputerColSize;
      }
      set {
        _ComputerColSize = Math.Max(8, Math.Min(50, value));
      }
    }
    internal static int _ComputerColSize;
    
    internal static string _ComputerColContent {
      get {
        if (_UserId.Length >= ComputerColSize) {
          return _Computer.Left(ComputerColSize);
        } else {
          return _Computer.PadRight(ComputerColSize, '.');
        }
      }
    }

    internal object syncIt = new object();

    internal const char CRLF_REPLACEMENT = '\r';

    #endregion Private variables

    #region Properties
    /// <summary>
    /// Indicate whether or not the current userid will be displayed in front of every line (default=false)
    /// </summary>
    public bool DisplayUserId = false;

    /// <summary>
    /// Indicate whether or not the current computer will be displayed in front of every line (default=false)
    /// </summary>
    public bool DisplayComputerName = false;

    
    #endregion Properties

    #region Constructors
    static TExtendedTraceListenerBase() {
      UserIdColSize = 20;
      ComputerColSize = 20;
    }

    /// <summary>
    /// Basic constructor
    /// </summary>
    public TExtendedTraceListenerBase() : base() {
      
    }
    #endregion Constructors

    #region Public methods
    /// <summary>
    /// Writes a message into log file with severity 'information'. If the message contains '\n', the line will be splitted.
    /// </summary>
    /// <param name="message">The message to add to the log</param>
    public override void WriteLine(string message) {
      WriteLine(message, Severity.Information);
    }

    /// <summary>
    /// Writes a message into log file with severity 'information'. If the message contains '\n', the line will be splitted.
    /// </summary>
    /// <param name="message">The message</param>
    public override void Write(string message) {
      this.WriteLine(message, Severity.Information);
    }

    /// <summary>
    /// Writes a message into log file with severity. If the message contains '\n', the line will be splitted.
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="severity">the severity of the message</param>
    public override void Write(string message, string severity) {
      this.WriteLine(message, severity);
    }
    #endregion Public methods

    #region Protected methods
    /// <summary>
    /// Builds a log line from components and optional components
    /// </summary>
    /// <param name="message">The message itself</param>
    /// <param name="severity">The severity (category) of the message</param>
    /// <param name="IndentSpace">How much do we indent</param>
    /// <returns></returns>
    protected string _BuildLogLine(string message, string severity, string IndentSpace) {
      StringBuilder RetVal = new StringBuilder(80);
      RetVal.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
      if (DisplayComputerName) {
        RetVal.Append("|");
        RetVal.Append(_ComputerColContent);
      }
      if (DisplayUserId) {
        RetVal.Append("|");
        RetVal.Append(_UserIdColContent);
      }
      RetVal.Append("|");
      RetVal.Append(severity.PadRight(7, '.'));
      RetVal.Append("|");
      RetVal.Append(IndentSpace);
      RetVal.AppendLine(message);
      return RetVal.ToString();
    }
    #endregion Protected methods
  }
}
