using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools {
  /// <summary>
  /// EventArgs holding both a boolean value and a message
  /// </summary>
  public class BoolAndMessageEventArgs : EventArgs {
    /// <summary>
    /// Boolean result to pass
    /// </summary>
    public bool Result { get; protected set; }
    /// <summary>
    /// A message to better describe the result (e.g. Operation failed because ...)
    /// </summary>
    public string Message { get; protected set; }
    /// <summary>
    /// Parameterless constructor
    /// </summary>
    public BoolAndMessageEventArgs() {
      Result = false;
      Message = "";
    }
    /// <summary>
    /// Constructor with result and message
    /// </summary>
    /// <param name="result">The boolean result to pass</param>
    /// <param name="message">The message</param>
    public BoolAndMessageEventArgs(bool result, string message = "") {
      Result = result;
      Message = message;
    }
  }
}
